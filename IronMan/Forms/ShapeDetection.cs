using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;
using DirectShowLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Xml.Serialization;
using System.Xml;
using IronMan.Forms;

namespace IronMan
{
    public partial class ShapeDetection : Form
    {
        public ShapeDetection()
        {
            InitializeComponent();
            IMC = new IronManConfig();
        }
        private IronManConfig IMC { get; set; }
        private Image<Bgr, Byte> SourceImg;
        private Image<Gray, Byte> Type1Img;
        private Image<Gray, Byte> Type2Img;
        public List<PickupObject> PickupObjects;
        public PickupObject RobotLocation;
        private bool TimerInUse = false;
        private System.Timers.Timer timer;

        #region Image Settings
        private int ImageWidth = 1280;
        private int ImageHeight = 720;

        #endregion

        #region Camera settings
        private int CameraBrightness = 0;
        private int CameraContrast = 0;
        private int CameraSharpness = 0;
        #endregion
        #region Camera Variables
        private VideoCapture CamCapture = null; //kamera
        private DsDevice[] SystemCameras; //lista svih kamera koje su dostupne
        #endregion

        #region Serial Port Config
        private SerialPort serial { get; set; }
        private string SerialData { get; set; }
        #endregion

        public void DetectRectangles(bool Type1)
        {
            //Prvo ispisemo podatke o lokaciji robota pa onda oblike
            //DisplayObjectInfo(RobotLocation, true);

            double sizeTresholdMin = double.Parse(tbSizeMin.Text);
            double sizeTresholdMax = double.Parse(tbSizeMax.Text);
            double cannyThreshold = double.Parse(tbCannyTreshold.Text);
            double cannyThresholdLinking = double.Parse(tbCannyTresholdLink.Text);

            UMat cannyEdges = new UMat();
            if (Type1)
                CvInvoke.Canny(Type1Img, cannyEdges, cannyThreshold, cannyThresholdLinking);
            else
                CvInvoke.Canny(Type2Img, cannyEdges, cannyThreshold, cannyThresholdLinking);

            // Petlja za prepoznavanje kontura objekta

            List<RotatedRect> boxList = new List<RotatedRect>();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                for (int i = 0; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        double approxContourSize = CvInvoke.ContourArea(approxContour, true);

                        //u obzir se uzimaju samo oblici cija je povrsina veca od odredjene
                        if (approxContourSize >= sizeTresholdMin && approxContourSize <= sizeTresholdMax)
                        {
                            if (approxContour.Size == 4) //Ako oblik ima 4 ugla onda je kvadrat
                            {
                                // potrebno je ustanoviti da li su uglovi u određenom rasponu [80, 100] stepeni
                                bool isRectangle = true;
                                LineSegment2D[] edges = PointCollection.PolyLine(approxContour.ToArray(), true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 70 || angle > 110) { isRectangle = false; break; }
                                }

                                if (isRectangle)
                                {
                                    RotatedRect tempRect = CvInvoke.MinAreaRect(approxContour);
                                    boxList.Add(tempRect);
                                    PickupObject obj = new PickupObject();
                                    obj.CenterX = Convert.ToInt32(tempRect.Center.X);
                                    obj.CenterY = Convert.ToInt32(tempRect.Center.Y);
                                    obj.Size = approxContourSize;
                                    obj.Type = Type1 ? "Type 1" : "Type 2";
                                    obj.Angle = tempRect.Angle;
                                    PickupObjects.Add(obj);
                                    DisplayObjectInfo(obj, false);
                                }
                            }
                        }
                    }
                }
            }
            //prikazemo oblik gdje je smjestena baza robota
            SourceImg.Draw(new Rectangle(IMC.RobotShapeX, IMC.RobotShapeY, IMC.RobotShapeWidth, IMC.RobotShapeHeight), new Bgr(Color.Yellow), 4);
            DrawWorkingArea();

            originalImageBox.Image = SourceImg.ToBitmap();
            originalImageBox.Refresh();

            //prikaz objekata
            Image<Bgr, Byte> RectangleImage = SourceImg.CopyBlank();
            foreach (RotatedRect box in boxList)
                RectangleImage.Draw(box, new Bgr(Color.Red), 4);

            if (Type1)
                Type1mageBox.Image = RectangleImage.ToBitmap();
            else
                Type2ImageBox.Image = RectangleImage.ToBitmap();
        }

        public Image<Gray, byte> FilterRectangles(bool Type1)
        {
            // 1. Pretvorimo sliku u HSV
            using (Image<Hsv, byte> hsv = SourceImg.Convert<Hsv, byte>())
            {
                // 2. Dobijemo 3 kanala (hue, saturation and value)
                Image<Gray, byte>[] channels = hsv.Split();

                try
                {   //kanal[0] je hue //kanal[2] je vrijednost.
                    Image<Gray, byte> Huefilter;
                    Image<Gray, byte> Valfilter;
                    if (Type1)
                    {
                        //filtriramo zeljenu boju
                        Huefilter = channels[0].InRange(new Gray(IMC.Type1HueMin), new Gray(IMC.Type1HueMax));
                        //filtriramo ostale boje
                        Valfilter = channels[2].InRange(new Gray(IMC.Type1ValMin), new Gray(IMC.Type1ValMax));
                    }
                    else
                    {
                        Huefilter = channels[0].InRange(new Gray(IMC.Type2HueMin), new Gray(IMC.Type2HueMax));
                        Valfilter = channels[2].InRange(new Gray(IMC.Type2ValMin), new Gray(IMC.Type2ValMax));
                    }

                    // 3. vratimo spojenu sliku
                    Image<Gray, byte> FinalImg = Huefilter.And(Valfilter);
                    return FinalImg;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return null;
                }
                finally
                {
                    channels[1].Dispose();
                    channels[2].Dispose();
                }
            }
        }

        private void DrawWorkingArea()
        {

            SourceImg.DrawPolyline(new Point[] { IMC.WAMinStart, IMC.WAMinMiddle, IMC.WAMinEnd }, true, new Bgr(Color.Yellow), 4);

        }

        private void ReadFromImage(object sender, EventArgs e)
        {
            tbCoordinates.Text = "";
            Stopwatch watch = Stopwatch.StartNew();
            watch.Start();

            SourceImg = new Image<Bgr, byte>(fileNameTextBox.Text).Resize(ImageWidth, ImageHeight, Emgu.CV.CvEnum.Inter.Linear, true);
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);
            DisplayObjectInfo(RobotLocation, true);
            DetectRectangles(true);
            DetectRectangles(false);
            watch.Stop();

            label2.Text = (String.Format("Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
        }

        private void ReadFromWebcam(object sender, EventArgs e)
        {
            tbCoordinates.Text = "";
            Stopwatch watch = Stopwatch.StartNew();
            watch.Start();
            PickupObjects = new List<PickupObject>();
            //ovdje ide parsiranje slike sa kamere u SourceImg
            SourceImg = CamCapture.QueryFrame().ToImage<Bgr, Byte>();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);
            DisplayObjectInfo(RobotLocation, true);
            DetectRectangles(true);
            DetectRectangles(false);
            watch.Stop();
            button3_Click(null, null);
            label2.Text = (String.Format("Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
        }

        private void ShapeDetection_Load(object sender, EventArgs e)
        {
            IMC = ReadFromXML();
            
            Servo1Scroll.Maximum = IMC.Servo1Max;
            Servo1Scroll.Minimum = IMC.Servo1Min;
            Servo2Scroll.Maximum = IMC.Servo2Max;
            Servo2Scroll.Minimum = IMC.Servo2Min;
            Servo3Scroll.Maximum = IMC.Servo3Max;
            Servo3Scroll.Minimum = IMC.Servo3Min;
            Servo4Scroll.Maximum = IMC.Servo4Max;
            Servo4Scroll.Minimum = IMC.Servo4Min;
            Servo5Scroll.Maximum = IMC.Servo5Max;
            Servo5Scroll.Minimum = IMC.Servo5Min;

            cbSaveValues.Checked = IMC.UsePreviousValues;
            if(cbSaveValues.Checked)
            {
                Servo1Scroll.Value = IMC.Servo1Val;
                Servo2Scroll.Value = IMC.Servo2Val;
                Servo3Scroll.Value = IMC.Servo3Val;
                Servo4Scroll.Value = IMC.Servo4Val;
                Servo5Scroll.Value = IMC.Servo5Val;
                lblServo1Value.Text = IMC.Servo1Val.ToString();
                lblServo2Value.Text = IMC.Servo2Val.ToString();
                lblServo3Value.Text = IMC.Servo3Val.ToString();
                lblServo4Value.Text = IMC.Servo4Val.ToString();
                lblServo5Value.Text = IMC.Servo5Val.ToString();
            }
            else
            {
                Servo1Scroll.Value = IMC.Servo1Def;
                Servo2Scroll.Value = IMC.Servo2Def;
                Servo3Scroll.Value = IMC.Servo3Def;
                Servo4Scroll.Value = IMC.Servo4Def;
                Servo5Scroll.Value = IMC.Servo5Def;
                lblServo1Value.Text = IMC.Servo1Def.ToString();
                lblServo2Value.Text = IMC.Servo2Def.ToString();
                lblServo3Value.Text = IMC.Servo3Def.ToString();
                lblServo4Value.Text = IMC.Servo4Def.ToString();
                lblServo5Value.Text = IMC.Servo5Def.ToString();
            }
            //IZ DO SADA NE RAZJASNJENIH RAZLOGA NULIRA SVE VRIJEDNOSTI Type1hue i Type1Val ako Bar-u dodjelujem vrijednost direktno
            // npr Bar1.Value = IMC.Type1HueMin; tada ce Type1HueMax i svi ostali postati = 0;

            int Type1HueMin = IMC.Type1HueMin;
            int Type1HueMax = IMC.Type1HueMax;
            int Type1ValMin = IMC.Type1ValMin;
            int Type1ValMax = IMC.Type1ValMax;
            int Type2HueMin = IMC.Type2HueMin;
            int Type2HueMax = IMC.Type2HueMax;
            int Type2ValMin = IMC.Type2ValMin;
            int Type2ValMax = IMC.Type2ValMax;
            Bar1.Value = Type1HueMin;
            Bar2.Value = Type1HueMax;
            Bar3.Value = Type1ValMin;
            Bar4.Value = Type1ValMax;
            Bar5.Value = Type2HueMin;
            Bar6.Value = Type2HueMax;
            Bar7.Value = Type2ValMin;
            Bar8.Value = Type2ValMax;

            label3.Text = "Min: " + Bar1.Value.ToString();
            label4.Text = "Max: " + Bar2.Value.ToString();
            label5.Text = "Min: " + Bar3.Value.ToString();
            label6.Text = "Max: " + Bar4.Value.ToString();
            label7.Text = "Min: " + Bar5.Value.ToString();
            label8.Text = "Max: " + Bar6.Value.ToString();
            label9.Text = "Min: " + Bar7.Value.ToString();
            label10.Text = "Max: " + Bar8.Value.ToString();

            PickupObjects = new List<PickupObject>();

            ReadRobotPosition(null, null);

            GetCamerasList();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);

            //timer kao automatski okidac za uzimanje uzorka slike
            timer = new System.Timers.Timer(800);
            timer.SynchronizingObject = this;
            timer.Elapsed += HandleTimerElapsed;

            cbSerialPort.SelectedIndex = 8;
            cbBaudRate.SelectedIndex = 9;
            SerialData = "";
            serial = new SerialPort();
        }

        public void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ReadFromWebcam(null, null);
        }

        private void GetCamerasList()
        {
            SystemCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (SystemCameras == null) return;

            button1.Enabled = true;
            CamCapture = new VideoCapture(0);

            cbCameras.DataSource = SystemCameras;
            cbCameras.DisplayMember = "Name";

        }

        private void SetType1Sliders(object sender, EventArgs e)
        {
            IMC.Type1HueMin = Bar1.Value;
            IMC.Type1HueMax = Bar2.Value;
            IMC.Type1ValMin = Bar3.Value;
            IMC.Type1ValMax = Bar4.Value;
            label3.Text = "Min: " + IMC.Type1HueMin.ToString();
            label4.Text = "Max: " + IMC.Type1HueMax.ToString();
            label5.Text = "Min: " + IMC.Type1ValMin.ToString();
            label6.Text = "Max: " + IMC.Type1ValMax.ToString();
            if (SourceImg == null) return;
            Type1Img = FilterRectangles(true);
            Type1mageBox.Image = Type1Img.ToBitmap();
        }

        private void SetType2Sliders(object sender, EventArgs e)
        {
            IMC.Type2HueMin = Bar5.Value;
            IMC.Type2HueMax = Bar6.Value;
            IMC.Type2ValMin = Bar7.Value;
            IMC.Type2ValMax = Bar8.Value;
            label7.Text = "Min: " + IMC.Type2HueMin.ToString();
            label8.Text = "Max: " + IMC.Type2HueMax.ToString();
            label9.Text = "Min: " + IMC.Type2ValMin.ToString();
            label10.Text = "Max: " + IMC.Type2ValMax.ToString();

            if (SourceImg == null) return;
            Type2Img = FilterRectangles(false);
            Type2ImageBox.Image = Type2Img.ToBitmap();
        }

        private void cbCameras_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CamCapture = new VideoCapture(cbCameras.SelectedIndex);
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            if (TimerInUse)
            {
                TimerInUse = false;
                btnTimer.Text = "Start";
                timer.Enabled = false;
                timer.Stop();
                return;
            }
            if (!TimerInUse)
            {
                TimerInUse = true;
                btnTimer.Text = "Stop";
                timer.Enabled = true;
                timer.Start();
            }
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            OpenSerialPort();
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (serial.IsOpen)
                serial.Close();
            lblPortStatus.Text = serial.IsOpen ? "Open" : "Closed";
        }

        private void ReadRobotPosition(object sender, EventArgs e)
        {
            IMC.RobotShapeHeight = int.Parse(tbRobotPosHeight.Text);
            IMC.RobotShapeWidth = int.Parse(tbRobotPosWidth.Text);
            IMC.RobotShapeX = int.Parse(tbRobotPosX.Text);
            IMC.RobotShapeY = int.Parse(tbRobotPosY.Text);

            if (RobotLocation == null) RobotLocation = new PickupObject();
            RobotLocation.CenterX = IMC.RobotShapeX + IMC.RobotShapeWidth / 2;
            RobotLocation.CenterY = IMC.RobotShapeY + IMC.RobotShapeHeight / 2;
            RobotLocation.Type = "Robot";
        }

        private void DisplayObjectInfo(PickupObject obj, bool IsRobot)
        {
            if (IsRobot)
            {
                tbCoordinates.Text += $"Type: {obj.Type}" + Environment.NewLine;
                tbCoordinates.Text += $"Center: {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
            }
            else
            {
                tbCoordinates.Text += $"Type: {obj.Type}" + Environment.NewLine;
                tbCoordinates.Text += $"Size: {obj.Size}" + Environment.NewLine;
                tbCoordinates.Text += $"Angle: {obj.Angle}" + Environment.NewLine;
                tbCoordinates.Text += $"Center: {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (PickupObjects.Count == 0) return;

            int ServoPos = ConvertRange(0, ImageWidth, 30, 150, PickupObjects[0].CenterX) + 20;
            SendCommands("Servo1", ServoPos.ToString());
            label28.Text = PickupObjects[0].CenterY + " -> " + ServoPos.ToString();
            return;

            if (PickupObjects[0].CenterX > RobotLocation.CenterX)
            {
                int Razlika = PickupObjects[0].CenterX - RobotLocation.CenterX;
                ServoPos = ConvertRange(0, ImageWidth / 2, 30, 90, Razlika);
                SendCommands("Servo1", ServoPos.ToString());
                label28.Text = PickupObjects[0].CenterY + " -> " + ServoPos.ToString();
            }
            else
            {
                int Razlika = RobotLocation.CenterX - PickupObjects[0].CenterX;
                ServoPos = ConvertRange(0, ImageWidth / 2, 90, 150, Razlika);
                SendCommands("Servo1", ServoPos.ToString());
                label28.Text = PickupObjects[0].CenterY + " -> " + ServoPos.ToString();
            }


        }

        public static int ConvertRange(
        int input_start, int input_end, // Izvorni raspon
        int output_start, int output_end, // Ciljani raspon
        int value) // vrijednost za pretvoriti
        {
            double slope = (double)(output_end - output_start) / (input_end - input_start);
            return (int)(output_start + (slope * (value - input_start)));
        }

        public float MapValue(float a0, float a1, float b0, float b1, float a)
        {
            return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
            //low2 + (value - low1) * (high2 - low2) / (high1 - low1)
        }

        private void Servo1Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands("Servo1", Servo1Scroll.Value.ToString());
            lblServo1Value.Text = Servo1Scroll.Value.ToString();
        }

        private void Servo2Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands("Servo2", Servo2Scroll.Value.ToString());
            lblServo2Value.Text = Servo2Scroll.Value.ToString();
        }

        private void Servo3Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands("Servo3", Servo3Scroll.Value.ToString());
            lblServo3Value.Text = Servo3Scroll.Value.ToString();
        }

        private void Servo4Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands("Servo4", Servo4Scroll.Value.ToString());
            lblServo4Value.Text = Servo4Scroll.Value.ToString();
        }

        private void Servo5Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands("Servo5", Servo5Scroll.Value.ToString());
            lblServo5Value.Text = Servo5Scroll.Value.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendCommands("Servo1", IMC.Servo1Def.ToString());
            SendCommands("Servo2", IMC.Servo2Def.ToString());
            SendCommands("Servo3", IMC.Servo3Def.ToString());
            SendCommands("Servo4", IMC.Servo4Def.ToString());
            SendCommands("Servo5", IMC.Servo5Def.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShapeDetection_Load(sender, e);
            //SendCommands("Servo1", "28"); Thread.Sleep(2000);
            //SendCommands("Servo3", "120"); Thread.Sleep(2000);
            //SendCommands("Servo2", "101"); Thread.Sleep(2000);
            //SendCommands("Servo5", "29"); Thread.Sleep(2000);

            //SendCommands("Servo2", "86"); Thread.Sleep(2000);
            //SendCommands("Servo1", "90"); Thread.Sleep(2000);
            //SendCommands("Servo2", "122"); Thread.Sleep(2000);
            //SendCommands("Servo3", "80"); Thread.Sleep(2000);
            //SendCommands("Servo5", "55"); Thread.Sleep(2000);
            ////reset
            //SendCommands("Servo1", "92"); Thread.Sleep(100);
            //SendCommands("Servo2", "5"); Thread.Sleep(100);
            //SendCommands("Servo3", "70"); Thread.Sleep(100);
            //SendCommands("Servo4", "6"); Thread.Sleep(100);
            //SendCommands("Servo5", "50"); Thread.Sleep(100);
        }

        private void OpenSerialPort()
        {
            if (serial != null)
                if (serial.IsOpen) serial.Close();
            try
            {
                serial = new SerialPort(cbSerialPort.Text, int.Parse(cbBaudRate.Text), Parity.None, 8, StopBits.One);
                serial.Open();
                //serial.DiscardOutBuffer();
                //serial.DiscardInBuffer();
                //serial.DataReceived += ReadSerialData;
                //SerialData = "";
                lblPortStatus.Text = serial.IsOpen ? "Opened" : "Closed";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendCommands(string ServoNumber, string Value)
        {
            if (serial.IsOpen)
                serial.WriteLine($"<{ServoNumber},{Value}>");
        }

        private void ShapeDetection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!serial.IsOpen)
                serial.Close();
            SaveAppConfig();
        }

        private void SaveAppConfig()
        {
            IMC.Type1HueMin = Bar1.Value;
            IMC.Type1HueMax = Bar2.Value;
            IMC.Type1ValMin = Bar3.Value;
            IMC.Type1ValMax = Bar4.Value;

            IMC.Type2HueMin = Bar5.Value;
            IMC.Type2HueMax = Bar6.Value;
            IMC.Type2ValMin = Bar7.Value;
            IMC.Type2ValMax = Bar8.Value;

            

            IMC.SizeTresholxMax = int.Parse(tbSizeMax.Text);
            IMC.SizeTresholdMin = int.Parse(tbSizeMin.Text);
            IMC.CannyTreshold = int.Parse(tbCannyTreshold.Text);
            IMC.CannyTresholdLinking = int.Parse(tbCannyTresholdLink.Text);

            IMC.RobotShapeX = int.Parse(tbRobotPosX.Text);
            IMC.RobotShapeY = int.Parse(tbRobotPosY.Text);
            IMC.RobotShapeWidth = int.Parse(tbRobotPosWidth.Text);
            IMC.RobotShapeHeight = int.Parse(tbRobotPosHeight.Text);

            IMC.ComPortIndex = cbSerialPort.SelectedIndex;
            IMC.BaudRateIndex = cbBaudRate.SelectedIndex;

            IMC.Servo1Min = Servo1Scroll.Minimum;
            IMC.Servo2Min = Servo2Scroll.Minimum;
            IMC.Servo3Min = Servo3Scroll.Minimum;
            IMC.Servo4Min = Servo4Scroll.Minimum;
            IMC.Servo5Min = Servo5Scroll.Minimum;

            IMC.Servo1Max = Servo1Scroll.Maximum;
            IMC.Servo2Max = Servo2Scroll.Maximum;
            IMC.Servo3Max = Servo3Scroll.Maximum;
            IMC.Servo4Max = Servo4Scroll.Maximum;
            IMC.Servo5Max = Servo5Scroll.Maximum;

            IMC.Servo1Val = Servo1Scroll.Value;
            IMC.Servo2Val = Servo2Scroll.Value;
            IMC.Servo3Val = Servo3Scroll.Value;
            IMC.Servo4Val = Servo4Scroll.Value;
            IMC.Servo5Val = Servo5Scroll.Value;
            IMC.UsePreviousValues = cbSaveValues.Checked;

            SaveToXML<IronManConfig>(IMC);
        }

        public void SaveToXML<T>(T serializableObject)
        {
            if (serializableObject == null) return;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                //XmlSerializer serializer = XmlSerializer.FromTypes(new[] { typeof(IronManConfig) })[0];
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save("IronManConfig.xml");
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public IronManConfig ReadFromXML()
        {
            IronManConfig objectOut = new IronManConfig();

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("IronManConfig.xml");
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(IronManConfig));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (IronManConfig)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return objectOut;
        }

        private void btnOpenRobotConfig_Click(object sender, EventArgs e)
        {
            FrmRobotConfig frm = new FrmRobotConfig(IMC);
            frm.ShowDialog();
        }
    }
}
