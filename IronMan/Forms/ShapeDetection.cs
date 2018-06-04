using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Xml.Serialization;
using System.Xml;
using IRIS.Forms;
using IRIS.Classes;
using System.Drawing.Imaging;

namespace IRIS
{
    public partial class ShapeDetection : Form
    {
        public ShapeDetection()
        {
            InitializeComponent();
            IRC = ReadFromXML<IRISConfig>("Config.xml");
            PickupList = new List<PickupPoint>();
            PickupObjects = new List<PickupObject>();
            serial = new SerialPort();
        }
        private IRISConfig IRC { get; set; }
        private Image<Bgr, Byte> SourceImg { get; set; }
        private Image<Gray, Byte> Type1Img { get; set; }
        private Image<Gray, Byte> Type2Img { get; set; }
        private List<PickupObject> PickupObjects { get; set; }
        private PickupObject RobotLocation { get; set; }
        //private List<List<PickupPoint>> PickupMatrix { get; set; }
        private List<PickupPoint> PickupList { get; set; }
        private Point[] WorkingAreaPoints { get; set; }
        private bool TimerInUse { get; set; }
        private System.Timers.Timer timer { get; set; }

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
                                    obj.InRange = IsObjectInRange(new Point(obj.CenterX, obj.CenterY));
                                    obj.Distance = MeasureDistance(obj);
                                    PickupObjects.Add(obj);
                                    DisplayObjectInfo(obj, false);
                                }
                            }
                        }
                    }
                }
            }
            //prikazemo oblik gdje je smjestena baza robota i radno podrucje
            DrawWorkingArea();

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
                        Huefilter = channels[0].InRange(new Gray(IRC.Type1HueMin), new Gray(IRC.Type1HueMax));
                        //filtriramo ostale boje
                        Valfilter = channels[2].InRange(new Gray(IRC.Type1ValMin), new Gray(IRC.Type1ValMax));
                    }
                    else
                    {
                        Huefilter = channels[0].InRange(new Gray(IRC.Type2HueMin), new Gray(IRC.Type2HueMax));
                        Valfilter = channels[2].InRange(new Gray(IRC.Type2ValMin), new Gray(IRC.Type2ValMax));
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
            Image<Bgr, byte> NewSourceImg = new Image<Bgr, byte>(SourceImg.Bitmap);
            NewSourceImg.Draw(new Rectangle(IRC.RobotShapeX, IRC.RobotShapeY, IRC.RobotShapeWidth, IRC.RobotShapeHeight), new Bgr(Color.Yellow), 4);
            WorkingAreaPoints = new Point[] {
             IRC.WAMinStart,
             IRC.WAMinMiddle,
             IRC.WAMinEnd,
             IRC.WAMaxEnd,
             IRC.WAMaxMiddle,
             IRC.WAMaxStart
            };
            NewSourceImg.DrawPolyline(WorkingAreaPoints, true, new Bgr(Color.Yellow), 4);

            originalImageBox.Image = NewSourceImg.ToBitmap();
            originalImageBox.Refresh();
        }

        private bool IsObjectInRange(Point point)
        {
            if (WorkingAreaPoints == null) return false;
            bool isInside = false;
            for (int i = 0, j = WorkingAreaPoints.Length - 1; i < WorkingAreaPoints.Length; j = i++)
            {
                if (((WorkingAreaPoints[i].Y > point.Y) != (WorkingAreaPoints[j].Y > point.Y)) &&
                (point.X < (WorkingAreaPoints[j].X - WorkingAreaPoints[i].X) * (point.Y - WorkingAreaPoints[i].Y) / (WorkingAreaPoints[j].Y - WorkingAreaPoints[i].Y) + WorkingAreaPoints[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
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
            SourceImg = CamCapture.QueryFrame().ToImage<Bgr, Byte>();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);
            DisplayObjectInfo(RobotLocation, true);
            DetectRectangles(true);
            DetectRectangles(false);
            watch.Stop();
            label2.Text = (String.Format("Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
            if(cbAutoSort.Checked && PickupObjects.Count > 0 )
                SortObjects();
        }

        private void ShapeDetection_Load(object sender, EventArgs e)
        {
            //PickupMatrix = ReadFromXML<List<List<PickupPoint>>>("ListOfPickupPoints.xml");
            PickupList = ReadFromXML<List<PickupPoint>>("ListOfPickupPoints.xml");
            //PickupList.Add(new PickupPoint() { Distance = 300, Servo2Val = 137, Servo3Val = 85 });
            Servo1Scroll.Maximum = IRC.ServoMaxPosition.Servo1Val;
            Servo1Scroll.Minimum = IRC.ServoMinPosition.Servo1Val;
            Servo2Scroll.Maximum = IRC.ServoMaxPosition.Servo2Val;
            Servo2Scroll.Minimum = IRC.ServoMinPosition.Servo2Val;
            Servo3Scroll.Maximum = IRC.ServoMaxPosition.Servo3Val;
            Servo3Scroll.Minimum = IRC.ServoMinPosition.Servo3Val;
            Servo4Scroll.Maximum = IRC.ServoMaxPosition.Servo4Val;
            Servo4Scroll.Minimum = IRC.ServoMinPosition.Servo4Val;
            Servo5Scroll.Maximum = IRC.ServoMaxPosition.Servo5Val;
            Servo5Scroll.Minimum = IRC.ServoMinPosition.Servo5Val;

            cbSaveValues.Checked = IRC.UsePreviousValues;

            if(cbSaveValues.Checked)
                DisplayServoValues(IRC.ServoPreviousPosition);
            else
                DisplayServoValues(IRC.ServoDefaultPosition);
            //IZ DO SADA NE RAZJASNJENIH RAZLOGA NULIRA SVE VRIJEDNOSTI Type1hue i Type1Val ako Bar-u dodjelujem vrijednost direktno
            // npr Bar1.Value = IMC.Type1HueMin; tada ce Type1HueMax i svi ostali postati = 0;

            int Type1HueMin = IRC.Type1HueMin;
            int Type1HueMax = IRC.Type1HueMax;
            int Type1ValMin = IRC.Type1ValMin;
            int Type1ValMax = IRC.Type1ValMax;
            int Type2HueMin = IRC.Type2HueMin;
            int Type2HueMax = IRC.Type2HueMax;
            int Type2ValMin = IRC.Type2ValMin;
            int Type2ValMax = IRC.Type2ValMax;
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

            tbSizeMax.Text = IRC.SizeTresholxMax.ToString();
            tbSizeMin.Text = IRC.SizeTresholdMin.ToString();
            tbCannyTreshold.Text = IRC.CannyTreshold.ToString();
            tbCannyTresholdLink.Text = IRC.CannyTresholdLinking.ToString();

            ReadRobotPosition(null, null);

            GetCamerasList();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);

            //timer kao automatski okidac za uzimanje uzorka slike
            timer = new System.Timers.Timer(IRC.CameraSpeedms);
            timer.SynchronizingObject = this;
            timer.Elapsed += HandleTimerElapsed;
            TimerInUse = false;
            cbSerialPort.SelectedIndex = IRC.ComPortIndex;
            cbBaudRate.SelectedIndex = IRC.BaudRateIndex;
            SerialData = "";
            GBProgramming.Visible = IRC.UseCalibrating;
            tbCannyTreshold.Visible = IRC.UseCalibrating;
            tbCannyTresholdLink.Visible = IRC.UseCalibrating;
            label15.Visible = IRC.UseCalibrating;
            label14.Visible = IRC.UseCalibrating;
            button7.Visible = IRC.UseCalibrating;
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
            IRC.Type1HueMin = Bar1.Value;
            IRC.Type1HueMax = Bar2.Value;
            IRC.Type1ValMin = Bar3.Value;
            IRC.Type1ValMax = Bar4.Value;
            label3.Text = "Min: " + IRC.Type1HueMin.ToString();
            label4.Text = "Max: " + IRC.Type1HueMax.ToString();
            label5.Text = "Min: " + IRC.Type1ValMin.ToString();
            label6.Text = "Max: " + IRC.Type1ValMax.ToString();
            if (SourceImg == null) return;
            Type1Img = FilterRectangles(true);
            Type1mageBox.Image = Type1Img.ToBitmap();
        }

        private void SetType2Sliders(object sender, EventArgs e)
        {
            IRC.Type2HueMin = Bar5.Value;
            IRC.Type2HueMax = Bar6.Value;
            IRC.Type2ValMin = Bar7.Value;
            IRC.Type2ValMax = Bar8.Value;
            label7.Text = "Min: " + IRC.Type2HueMin.ToString();
            label8.Text = "Max: " + IRC.Type2HueMax.ToString();
            label9.Text = "Min: " + IRC.Type2ValMin.ToString();
            label10.Text = "Max: " + IRC.Type2ValMax.ToString();

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
            OpenSerialPort(sender,e);
        }

        private void ReadRobotPosition(object sender, KeyEventArgs e)
        {
            if (RobotLocation == null) RobotLocation = new PickupObject();
            RobotLocation.CenterX = IRC.RobotShapeX + IRC.RobotShapeWidth / 2;
            RobotLocation.CenterY = IRC.RobotShapeY + IRC.RobotShapeHeight / 2;
            RobotLocation.Type = "Robot";
        }

        private void DisplayObjectInfo(PickupObject obj, bool IsRobot)
        {
            if (IsRobot)
            {
                tbCoordinates.Text += $"Type: {obj.Type}" + Environment.NewLine;
                tbCoordinates.Text += $"Center(X,Y): {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                if (IRC.UseCalibrating)
                {
                    SourceImg.DrawPolyline(new Point[2] { new Point(RobotLocation.CenterX, 0), new Point(RobotLocation.CenterX, RobotLocation.CenterY) }, true, new Bgr(Color.Green), 2);
                    SourceImg.DrawPolyline(new Point[3] {
                        new Point(0, RobotLocation.CenterY),
                        new Point(ImageWidth,RobotLocation.CenterY),
                        new Point(RobotLocation.CenterX, RobotLocation.CenterY),
                    }, true, new Bgr(Color.Green), 2);
                }
            }
            else
            {
                tbCoordinates.Text += $"Type: {obj.Type}, Size: {obj.Size}" + Environment.NewLine;
                tbCoordinates.Text += $"Center(X,Y): {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += $"In Range: {obj.InRange}" + Environment.NewLine;
                tbCoordinates.Text += $"Distance (mm): {(obj.Distance)}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SortObjects();
        }

        private void SortObjects()
        {
            if (!PickupObjects.Any()) return;

            foreach (PickupObject PO in PickupObjects.Where(X => X.InRange).OrderBy(X => X.Distance))
            {
                int Distance = MeasureDistance(PO);
                PickupPoint P = PickupList.Where(X => X.Distance == Distance).FirstOrDefault();
                if (P == null) { MessageBox.Show("Distance is not defined."); continue; }
                tbServoValues.Text += $"Type: {PO.Type}, Size: {PO.Size}" + Environment.NewLine;
                tbServoValues.Text += $"Center(X,Y): {PO.CenterX}, {PO.CenterY}" + Environment.NewLine;
                tbServoValues.Text += $"In Range: {PO.InRange}" + Environment.NewLine;
                tbServoValues.Text += $"Distance (mm): {(PO.Distance)}" + Environment.NewLine;
                tbServoValues.Text += "------------------------------" + Environment.NewLine;
                SendCommands("Servo5", "87"); Thread.Sleep(1500);
                SendCommands("Servo2", "86"); Thread.Sleep(1500);
                SendCommands("Servo3", "124"); Thread.Sleep(1500);
                SendCommands("Servo1", MeasureAngle(PO).ToString()); Thread.Sleep(1500);
                SendCommands("Servo3", P.Servo3Val.ToString()); Thread.Sleep(1500);
                SendCommands("Servo2", P.Servo2Val.ToString()); Thread.Sleep(1500);
                SendCommands("Servo5", "44"); Thread.Sleep(1500);
                GoToDropoffPos(PO.Type);
            }
            if(!cbAutoSort.Checked)
                GoToResetPos();
        }

        private int MeasureAngle(PickupObject PO)
        {
            //X1 je robot X2 je objekat
            //Korijen iz (x2-x1)^2 + (Y2-Y1)^2

            double SideC = (PO.CenterX > RobotLocation.CenterX) ? PO.CenterX - RobotLocation.CenterX : RobotLocation.CenterX - PO.CenterX;
            double SideB = Math.Sqrt(Math.Pow(PO.CenterX - RobotLocation.CenterX,2) + Math.Pow(PO.CenterY - (RobotLocation.CenterY + IRC.RobotShapeCenterRotation),2));
            double SideA = Math.Sqrt(Math.Pow(SideB, 2) - Math.Pow(SideC, 2));
            double Angle = Math.Acos(SideA / SideB) * 180 / 3.14;
            Angle = PO.CenterX > RobotLocation.CenterX ? IRC.ServoDefaultPosition.Servo1Val + Angle : IRC.ServoDefaultPosition.Servo1Val - Angle;
            return (int)Math.Round(Angle);
        }

        private int MeasureDistance(PickupObject PO)
        {
            double PPCM = IRC.PixelsPerCM;//Default 15.2144186; //Pixels per CM
            double SideB = Math.Sqrt(Math.Pow(PO.CenterX - RobotLocation.CenterX, 2) + Math.Pow(PO.CenterY - RobotLocation.CenterY, 2));
            SideB = (SideB / PPCM) * 10; // pretvorimo pixele u cm pa u mm
            double SideA = IRC.BaseHeightMM;
            double SideC = Math.Sqrt(Math.Pow(SideA, 2) + Math.Pow(SideB, 2));

            //---------------------------------------------------------------------------------------------------
            SideA = IRC.ArmLengthMM;
            SideB = IRC.BaseLengthMM;
            double AngleA = (Math.Pow(SideB, 2) + Math.Pow(SideC, 2) - Math.Pow(SideA, 2)) / (2 * SideB * SideC);
            AngleA = Math.Acos(AngleA) * 180 / 3.14;

            double AngleB = (Math.Pow(SideC, 2) + Math.Pow(SideA, 2) - Math.Pow(SideB, 2)) / (2 * SideC * SideA);
            AngleB = Math.Acos(AngleB) * 180 / 3.14;

            double AngleC = 180 - AngleB - AngleA;
            //---------------------------------------------------------------------------------------------------
            return (int)Math.Round(SideC);
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
            GoToResetPos();
        }

        private void GoToResetPos()
        {
            SendCommands("Servo3", IRC.ServoDefaultPosition.Servo3Val.ToString()); Thread.Sleep(600);
            SendCommands("Servo2", IRC.ServoDefaultPosition.Servo2Val.ToString()); Thread.Sleep(600);
            SendCommands("Servo1", IRC.ServoDefaultPosition.Servo1Val.ToString()); Thread.Sleep(600);
            SendCommands("Servo4", IRC.ServoDefaultPosition.Servo4Val.ToString()); Thread.Sleep(600);
            SendCommands("Servo5", IRC.ServoDefaultPosition.Servo5Val.ToString());
            DisplayServoValues(IRC.ServoDefaultPosition);
        }

        private void GoToDropoffPos(string Type)
        {
            if(Type == "Type 1")
            {
                SendCommands("Servo2", IRC.DropType1Position.Servo2Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo3", IRC.DropType1Position.Servo3Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo1", IRC.DropType1Position.Servo1Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo5", IRC.DropType1Position.Servo5Val.ToString()); Thread.Sleep(1000);
                DisplayServoValues(IRC.DropType1Position);
            }
            else
            {
                SendCommands("Servo2", IRC.DropType2Position.Servo2Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo3", IRC.DropType2Position.Servo3Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo1", IRC.DropType2Position.Servo1Val.ToString()); Thread.Sleep(1000);
                SendCommands("Servo5", IRC.DropType2Position.Servo5Val.ToString()); Thread.Sleep(1000);
                DisplayServoValues(IRC.DropType2Position);
            }
            
        }

        private void DisplayServoValues(PickupPoint PP)
        {
            Servo1Scroll.Value = PP.Servo1Val;
            Servo2Scroll.Value = PP.Servo2Val;
            Servo3Scroll.Value = PP.Servo3Val;
            Servo4Scroll.Value = PP.Servo4Val;
            Servo5Scroll.Value = PP.Servo5Val;

            lblServo1Value.Text = Servo1Scroll.Value.ToString();
            lblServo2Value.Text = Servo2Scroll.Value.ToString();
            lblServo3Value.Text = Servo3Scroll.Value.ToString();
            lblServo4Value.Text = Servo4Scroll.Value.ToString();
            lblServo5Value.Text = Servo5Scroll.Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShapeDetection_Load(sender, e);
        }

        private void OpenSerialPort(object sender, EventArgs e)
        {
            try
            {
                if (serial != null)
                    if (serial.IsOpen)
                    {
                        serial.Close();
                    }
                    else
                    {
                        serial = new SerialPort(cbSerialPort.Text, int.Parse(cbBaudRate.Text), Parity.None, 8, StopBits.One);
                        serial.Open();
                    }
                lblPortStatus.Text = serial.IsOpen ? "Opened" : "Closed";
                btnOpenSerial.Text = serial.IsOpen ? "Close Serial" : "Open Serial";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendCommands(string ServoNumber, string Value)
        {
            if (!serial.IsOpen) return;
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
            IRC.Type1HueMin = Bar1.Value;
            IRC.Type1HueMax = Bar2.Value;
            IRC.Type1ValMin = Bar3.Value;
            IRC.Type1ValMax = Bar4.Value;

            IRC.Type2HueMin = Bar5.Value;
            IRC.Type2HueMax = Bar6.Value;
            IRC.Type2ValMin = Bar7.Value;
            IRC.Type2ValMax = Bar8.Value;

            IRC.SizeTresholxMax = int.Parse(tbSizeMax.Text);
            IRC.SizeTresholdMin = int.Parse(tbSizeMin.Text);
            IRC.CannyTreshold = int.Parse(tbCannyTreshold.Text);
            IRC.CannyTresholdLinking = int.Parse(tbCannyTresholdLink.Text);

            IRC.ComPortIndex = cbSerialPort.SelectedIndex;
            IRC.BaudRateIndex = cbBaudRate.SelectedIndex;

            IRC.ServoPreviousPosition.Servo1Val = Servo1Scroll.Value;
            IRC.ServoPreviousPosition.Servo2Val = Servo2Scroll.Value;
            IRC.ServoPreviousPosition.Servo3Val = Servo3Scroll.Value;
            IRC.ServoPreviousPosition.Servo4Val = Servo4Scroll.Value;
            IRC.ServoPreviousPosition.Servo5Val = Servo5Scroll.Value;
            IRC.UsePreviousValues = cbSaveValues.Checked;

            SaveToXML<IRISConfig>(IRC, "Config.xml");
            SaveToXML<List<PickupPoint>>(PickupList, "ListOfPickupPoints.xml");
        }

        public void SaveToXML<T>(T serializableObject, string _FileName)
        {
            if (serializableObject == null) return;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(_FileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public T ReadFromXML<T>(string _FileName)
        {
            if (string.IsNullOrEmpty(_FileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(_FileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
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
            FrmRobotConfig frm = new FrmRobotConfig(IRC);
            frm.ShowDialog();
        }

        private void originalImageBox_DoubleClick(object sender, EventArgs e)
        {
            if (originalImageBox.Image == null) return;
            int Counter = 1;
            while(File.Exists($"SourceImg{Counter}.jpeg"))
            {
                Counter++;
            }
            originalImageBox.Image.Save($"SourceImg{Counter}.jpeg", ImageFormat.Jpeg);
            MessageBox.Show("Image Saved");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            double Samples = IRC.PickupPointSamples;
            double DeltaServo2 = (IRC.PickupPointEnd.Servo2Val - IRC.PickupPointStart.Servo2Val) / Samples;
            double DeltaServo3 = (IRC.PickupPointEnd.Servo3Val - IRC.PickupPointStart.Servo3Val) / Samples;
            double DeltaDistance = (IRC.PickupPointEnd.Distance - IRC.PickupPointStart.Distance) / Samples;
            PickupList = new List<PickupPoint>();
            for (int i = 0; i <= Samples; i++)
            {
                double NewDistance = IRC.PickupPointStart.Distance + (i * DeltaDistance);
                int Servo2 = (int)Math.Round(IRC.PickupPointStart.Servo2Val + (i * DeltaServo2));
                int Servo3 = (int)Math.Round(IRC.PickupPointStart.Servo3Val + (i * DeltaServo3));
                PickupList.Add(new PickupPoint { Distance = (int)Math.Round(NewDistance), Servo2Val = Servo2 , Servo3Val = Servo3});
            }
        }

        private void btnSearchMatrix_Click(object sender, EventArgs e)
        {
            //PickupPoint PP = PickupMatrix[int.Parse(tbMatrixX.Text)][int.Parse(tbMatrixY.Text)];
            //tbS1Val.Text = PP.Servo1Val.ToString();
            //tbS2Val.Text = PP.Servo2Val.ToString();
            //tbS3Val.Text = PP.Servo3Val.ToString();
            //tbEndX.Text = PP.EndX.ToString();
            //tbEndY.Text = PP.EndY.ToString();
            //tbStartX.Text = PP.StartX.ToString();
            //tbStartY.Text = PP.StartY.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FrmCameraSettings frm = new FrmCameraSettings();
            frm.ShowDialog();
        }
    }
}
