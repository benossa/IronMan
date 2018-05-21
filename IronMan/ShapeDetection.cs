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

namespace IronMan
{
    public partial class ShapeDetection : Form
    {
        public ShapeDetection()
        {
            InitializeComponent();
        }

        private Image<Bgr, Byte> SourceImg;
        private Image<Gray, Byte> Type1Img;
        private Image<Gray, Byte> Type2Img;
        public List<PickupObject> PickupObjects;
        public PickupObject RobotLocation;
        private bool TimerInUse = false;
        private System.Timers.Timer timer;

        #region Image Settings
        private int ImageWidth = 800;
        private int ImageHeight = 600;

        private int RobotShapeX = 0, RobotShapeY = 0, RobotShapeWidth = 0, RobotShapeHeight = 0;

        private double Type1HueMin = 75;  //80;
        private double Type1HueMax = 146; //145;
        private double Type1ValMin = 153; //150 def
        private double Type1ValMax = 255; //255

                                          //CRVENA
        private double Type2HueMin = 161; //161
        private double Type2HueMax = 243; //243
        private double Type2ValMin = 35; //35
        private double Type2ValMax = 255; //255
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
            if(Type1)
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
            SourceImg.Draw(new Rectangle(RobotShapeX, RobotShapeY, RobotShapeWidth, RobotShapeHeight), new Bgr(Color.Yellow), 4);

            originalImageBox.Image = SourceImg.ToBitmap(); 
            originalImageBox.Refresh();

            //prikaz objekata
            Image<Bgr, Byte> RectangleImage = SourceImg.CopyBlank();
            foreach (RotatedRect box in boxList)
                RectangleImage.Draw(box, new Bgr(Color.Red), 4);
            
            if(Type1)
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
                        Huefilter = channels[0].InRange(new Gray(Type1HueMin), new Gray(Type1HueMax));
                        //filtriramo ostale boje
                        Valfilter = channels[2].InRange(new Gray(Type1ValMin), new Gray(Type1ValMax));
                    }
                    else
                    {
                        Huefilter = channels[0].InRange(new Gray(Type2HueMin), new Gray(Type2HueMax));
                        Valfilter = channels[2].InRange(new Gray(Type2ValMin), new Gray(Type2ValMax));
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
            Bar1.Value = Convert.ToInt16(Type1HueMin);
            Bar2.Value = Convert.ToInt16(Type1HueMax);
            Bar3.Value = Convert.ToInt16(Type1ValMin);
            Bar4.Value = Convert.ToInt16(Type1ValMax);
            Bar5.Value = Convert.ToInt16(Type2HueMin);
            Bar6.Value = Convert.ToInt16(Type2HueMax);
            Bar7.Value = Convert.ToInt16(Type2ValMin);
            Bar8.Value = Convert.ToInt16(Type2ValMax);

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
            ReadFromWebcam(null,null);
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
            Type1HueMin = Bar1.Value;
            Type1HueMax = Bar2.Value;
            Type1ValMin = Bar3.Value;
            Type1ValMax = Bar4.Value;
            label3.Text = "Min: " + Type1HueMin.ToString();
            label4.Text = "Max: " + Type1HueMax.ToString();
            label5.Text = "Min: " + Type1ValMin.ToString();
            label6.Text = "Max: " + Type1ValMax.ToString();
            if (SourceImg == null) return;
            Type1Img = FilterRectangles(true);
            Type1mageBox.Image = Type1Img.ToBitmap();
        }

        private void SetType2Sliders(object sender, EventArgs e)
        {   
            Type2HueMin = Bar5.Value;
            Type2HueMax = Bar6.Value;
            Type2ValMin = Bar7.Value;
            Type2ValMax = Bar8.Value;
            label7.Text = "Min: " + Type2HueMin.ToString();
            label8.Text = "Max: " + Type2HueMax.ToString();
            label9.Text = "Min: " + Type2ValMin.ToString();
            label10.Text = "Max: " + Type2ValMax.ToString();

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
            if(!TimerInUse)
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
            RobotShapeHeight = int.Parse(RobotPosHeight.Text);
            RobotShapeWidth = int.Parse(RobotPosWidth.Text);
            RobotShapeX = int.Parse(RobotPosX.Text);
            RobotShapeY = int.Parse(RobotPosY.Text);

            if (RobotLocation == null) RobotLocation = new PickupObject();
            RobotLocation.CenterX = RobotShapeX + RobotShapeWidth / 2;
            RobotLocation.CenterY = RobotShapeY + RobotShapeHeight / 2;
            RobotLocation.Type = "Robot";
        }

        private void DisplayObjectInfo(PickupObject obj, bool IsRobot)
        {
            if(IsRobot)
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
            int ServoLoc = ConvertRange(0, ImageWidth, 2, 178, PickupObjects[0].CenterX) - 43;
            SendCommands("Servo1", ServoLoc.ToString());
            label28.Text = PickupObjects[0].CenterY + " -> " +ServoLoc.ToString();
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
        }
    }
}
