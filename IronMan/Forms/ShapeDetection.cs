﻿using Emgu.CV;
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
            PickupMatrix = new List<List<PickupPoint>>();

            for (int i = 0; i < IRC.MatrixSizeX; i++)
            {
                PickupMatrix.Add(new List<PickupPoint>());
                for (int j = 0; j < IRC.MatrixSizeY; j++)
                {
                    PickupMatrix[i].Add(new PickupPoint(0,0,0,false,i,j,0,0,0,0));
                }
            }
            PickupObjects = new List<PickupObject>();
            serial = new SerialPort();
        }
        private IRISConfig IRC { get; set; }
        private Image<Bgr, Byte> SourceImg { get; set; }
        private Image<Gray, Byte> Type1Img { get; set; }
        private Image<Gray, Byte> Type2Img { get; set; }
        private List<PickupObject> PickupObjects { get; set; }
        private PickupObject RobotLocation { get; set; }
        private List<List<PickupPoint>> PickupMatrix { get; set; }
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

            foreach (var PP in PickupMatrix)
            {
                foreach (PickupPoint PO in PP.Where(X=>X.IsValid))
                {
                    NewSourceImg.Draw(new Rectangle(PO.StartX,PO.StartY, PO.EndX - PO.StartX, PO.EndY - PO.StartY), new Bgr(Color.Blue), 2);
                }
            }
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
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);
            DisplayObjectInfo(RobotLocation, true);
            DetectRectangles(true);
            DetectRectangles(false);
            watch.Stop();
            //button3_Click(null, null);
            label2.Text = (String.Format("Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
        }

        private void ShapeDetection_Load(object sender, EventArgs e)
        {
            PickupMatrix = ReadFromXML<List<List<PickupPoint>>>("ListOfPickupPoints.xml");
            Servo1Scroll.Maximum = IRC.Servo1Max;
            Servo1Scroll.Minimum = IRC.Servo1Min;
            Servo2Scroll.Maximum = IRC.Servo2Max;
            Servo2Scroll.Minimum = IRC.Servo2Min;
            Servo3Scroll.Maximum = IRC.Servo3Max;
            Servo3Scroll.Minimum = IRC.Servo3Min;
            Servo4Scroll.Maximum = IRC.Servo4Max;
            Servo4Scroll.Minimum = IRC.Servo4Min;
            Servo5Scroll.Maximum = IRC.Servo5Max;
            Servo5Scroll.Minimum = IRC.Servo5Min;

            cbSaveValues.Checked = IRC.UsePreviousValues;
            if(cbSaveValues.Checked)
            {
                Servo1Scroll.Value = IRC.Servo1Val;
                Servo2Scroll.Value = IRC.Servo2Val;
                Servo3Scroll.Value = IRC.Servo3Val;
                Servo4Scroll.Value = IRC.Servo4Val;
                Servo5Scroll.Value = IRC.Servo5Val;
                lblServo1Value.Text = IRC.Servo1Val.ToString();
                lblServo2Value.Text = IRC.Servo2Val.ToString();
                lblServo3Value.Text = IRC.Servo3Val.ToString();
                lblServo4Value.Text = IRC.Servo4Val.ToString();
                lblServo5Value.Text = IRC.Servo5Val.ToString();
            }
            else
            {
                Servo1Scroll.Value = IRC.Servo1Def;
                Servo2Scroll.Value = IRC.Servo2Def;
                Servo3Scroll.Value = IRC.Servo3Def;
                Servo4Scroll.Value = IRC.Servo4Def;
                Servo5Scroll.Value = IRC.Servo5Def;
                lblServo1Value.Text = IRC.Servo1Def.ToString();
                lblServo2Value.Text = IRC.Servo2Def.ToString();
                lblServo3Value.Text = IRC.Servo3Def.ToString();
                lblServo4Value.Text = IRC.Servo4Def.ToString();
                lblServo5Value.Text = IRC.Servo5Def.ToString();
            }
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


            tbRobotPosX.Text = IRC.RobotShapeX.ToString();
            tbRobotPosY.Text = IRC.RobotShapeY.ToString();
            tbRobotPosWidth.Text = IRC.RobotShapeWidth.ToString();
            tbRobotPosHeight.Text = IRC.RobotShapeHeight.ToString();
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
            GBProgramming.Visible = IRC.UseProgramming;
            tbServoValues.Visible = IRC.UseProgramming;
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
            OpenSerialPort();
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (serial.IsOpen)
                serial.Close();
            lblPortStatus.Text = serial.IsOpen ? "Open" : "Closed";
        }

        private void ReadRobotPosition(object sender, KeyEventArgs e)
        {
            IRC.RobotShapeHeight = int.Parse(tbRobotPosHeight.Text);
            IRC.RobotShapeWidth = int.Parse(tbRobotPosWidth.Text);
            IRC.RobotShapeX = int.Parse(tbRobotPosX.Text);
            IRC.RobotShapeY = int.Parse(tbRobotPosY.Text);

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
                tbCoordinates.Text += $"Center: {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
            }
            else
            {
                obj.InRange = IsObjectInRange(new Point(obj.CenterX,obj.CenterY));
                //tbCoordinates.Text += $"Type: {obj.Type}" + Environment.NewLine;
                tbCoordinates.Text += $"Size: {obj.Size}" + Environment.NewLine;
                //tbCoordinates.Text += $"Angle: {obj.Angle}" + Environment.NewLine;
                tbCoordinates.Text += $"Center: {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                tbCoordinates.Text += $"In Range: {obj.InRange}" + Environment.NewLine;
                //tbCoordinates.Text += $"Servo1 Value: {MeasureAngle(obj)}" + Environment.NewLine;
                //tbCoordinates.Text += $"Distance (mm): {(obj)}" + Environment.NewLine;
                tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!PickupObjects.Any()) return;
            tbServoValues.Clear();

            foreach (PickupObject PO in PickupObjects)
            {
                //foreach (var Rows in PickupMatrix)
                //{
                    PickupPoint P = new PickupPoint(MeasureAngle(PO));
                    SendCommands("Servo5", "87"); Thread.Sleep(300);
                    SendCommands("Servo1", P.Servo1Val.ToString()); Thread.Sleep(300);
                    MeasureDistance(PO, P);
                    SendCommands("Servo3", P.Servo3Val.ToString()); Thread.Sleep(300);
                    SendCommands("Servo2", P.Servo2Val.ToString()); Thread.Sleep(300);
                    SendCommands("Servo5", "42"); Thread.Sleep(300);

                    //PickupPoint PP = Rows.Where(X => PO.CenterX >= X.StartX && PO.CenterX <= X.EndX && PO.CenterY >= X.StartY && PO.CenterY <= X.EndY).FirstOrDefault();
                    //if (PP != null && PO.InRange)
                    //{
                    //    if (PP.Servo1Val == 0 || PP.Servo2Val == 0 || PP.Servo3Val == 0)
                    //        continue;
                    //    PP.HasObject = true;
                    //    PP.ObjectType = PO.Type;
                    //    tbServoValues.Text += $"X: {PP.X}  Y: {PP.Y}  Type: {PP.ObjectType}" + Environment.NewLine;
                    //    tbServoValues.Text += $"StartX: {PP.StartX}   EndX: {PP.EndX}" + Environment.NewLine;
                    //    tbServoValues.Text += $"S1: {PP.Servo1Val}  S2: {PP.Servo2Val}  S3: {PP.Servo3Val}" + Environment.NewLine;
                    //    SendCommands("Servo5", "87"); Thread.Sleep(300);
                    //    SendCommands("Servo1", MeasureAngle(PO).ToString()); Thread.Sleep(300);
                    //    SendCommands("Servo3", PP.Servo3Val.ToString()); Thread.Sleep(300);
                    //    SendCommands("Servo2", PP.Servo2Val.ToString()); Thread.Sleep(300);
                    //    SendCommands("Servo5", "42"); Thread.Sleep(300);
                    //    GoToDropoffPos(PP.ObjectType);
                    //}
                //}
            }
            //GoToResetPos();
        }

        private int MeasureAngle(PickupObject PO)
        {
            //X1 je robot X2 je objekat
            //Korijen iz (x2-x1)^2 + (Y2-Y1)^2
            double SideC = (PO.CenterX > RobotLocation.CenterX) ? PO.CenterX - RobotLocation.CenterX : RobotLocation.CenterX - PO.CenterX;
            double SideB = Math.Sqrt(Math.Pow(PO.CenterX - RobotLocation.CenterX,2) + Math.Pow(PO.CenterY - RobotLocation.CenterY,2));
            double SideA = Math.Sqrt(Math.Pow(SideB, 2) - Math.Pow(SideC, 2));
            double Angle = Math.Acos(SideA / SideB) * 180 / 3.14;
            Angle = PO.CenterX > RobotLocation.CenterX ? IRC.Servo1Def + Angle : IRC.Servo1Def - Angle;
            return (int)Math.Round(Angle);
        }

        private void MeasureDistance(PickupObject PO, PickupPoint PP)
        {
            double PPCM = IRC.PixelsPerCM;//Default 15.2144186; //Pixels per CM
            double SideB = Math.Sqrt(Math.Pow(PO.CenterX - RobotLocation.CenterX, 2) + Math.Pow(PO.CenterY - RobotLocation.CenterY, 2));
            SideB = (SideB / PPCM) * 10; // pretvorimo pixele u cm pa u mm
            double SideA = IRC.BaseHeightMM;
            double SideC = Math.Sqrt(Math.Pow(SideA, 2) + Math.Pow(SideB, 2));
            //---------------------------------------------------------------------------------------------------
            double AngleA = (Math.Pow(IRC.BaseLengthMM, 2) + Math.Pow(SideC, 2) - Math.Pow(IRC.ArmLengthMM, 2)) / (2 * IRC.BaseLengthMM * SideC);
            AngleA = Math.Acos(AngleA) * 180 / 3.14;

            double AngleC = (Math.Pow(SideC, 2) + Math.Pow(IRC.ArmLengthMM, 2) - Math.Pow(IRC.BaseLengthMM, 2)) / (2 * SideC * IRC.ArmLengthMM);
            AngleC = Math.Acos(AngleC) * 180 / 3.14;

            double AngleB = 180 - AngleC - AngleA;
            //---------------------------------------------------------------------------------------------------
            PP.Servo2Val = (int)Math.Round(AngleA) + IRC.Servo2Zero;
            PP.Servo3Val = IRC.Servo3Zero - (int)Math.Round(AngleB);
            tbServoValues.Text += $"Servo2: {PP.Servo2Val}, Servo3: { PP.Servo3Val}" + Environment.NewLine;
            //tbServoValues.Text += $"Angle A: {Math.Round(AngleA, 3)}, Angle B: {Math.Round(AngleB, 3)}, Angle C: {Math.Round(AngleC, 3)}" + Environment.NewLine;
            //tbServoValues.Text += $"Side A: {IRC.ArmLengthMM}, Side B: {IRC.BaseLengthMM}, Side C: {Math.Round(SideC, 3)}" + Environment.NewLine;
            //return SideC.ToString();
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
            SendCommands("Servo3", IRC.Servo3Def.ToString()); Thread.Sleep(500);
            SendCommands("Servo2", IRC.Servo2Def.ToString()); Thread.Sleep(500);
            SendCommands("Servo1", IRC.Servo1Def.ToString()); Thread.Sleep(500);
            SendCommands("Servo4", IRC.Servo4Def.ToString()); Thread.Sleep(500);
            SendCommands("Servo5", IRC.Servo5Def.ToString()); Thread.Sleep(500);

            Servo1Scroll.Value = IRC.Servo1Def;
            Servo2Scroll.Value = IRC.Servo2Def;
            Servo3Scroll.Value = IRC.Servo3Def;
            Servo4Scroll.Value = IRC.Servo4Def;
            Servo5Scroll.Value = IRC.Servo5Def;
        }

        private void GoToDropoffPos(string Type)
        {
            if(Type == "Type 1")
            {
                SendCommands("Servo2", "60"); Thread.Sleep(500);
                SendCommands("Servo3", "67"); Thread.Sleep(500);
                SendCommands("Servo1", "170"); Thread.Sleep(500);
                SendCommands("Servo5", "87"); Thread.Sleep(500);

                Servo1Scroll.Value = IRC.Servo1Def;
                Servo2Scroll.Value = IRC.Servo2Def;
                Servo3Scroll.Value = IRC.Servo3Def;
                Servo4Scroll.Value = IRC.Servo4Def;
                Servo5Scroll.Value = IRC.Servo5Def;
            }
            else
            {

            }

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

            IRC.RobotShapeX = int.Parse(tbRobotPosX.Text);
            IRC.RobotShapeY = int.Parse(tbRobotPosY.Text);
            IRC.RobotShapeWidth = int.Parse(tbRobotPosWidth.Text);
            IRC.RobotShapeHeight = int.Parse(tbRobotPosHeight.Text);

            IRC.ComPortIndex = cbSerialPort.SelectedIndex;
            IRC.BaudRateIndex = cbBaudRate.SelectedIndex;

            IRC.Servo1Min = Servo1Scroll.Minimum;
            IRC.Servo2Min = Servo2Scroll.Minimum;
            IRC.Servo3Min = Servo3Scroll.Minimum;
            IRC.Servo4Min = Servo4Scroll.Minimum;
            IRC.Servo5Min = Servo5Scroll.Minimum;

            IRC.Servo1Max = Servo1Scroll.Maximum;
            IRC.Servo2Max = Servo2Scroll.Maximum;
            IRC.Servo3Max = Servo3Scroll.Maximum;
            IRC.Servo4Max = Servo4Scroll.Maximum;
            IRC.Servo5Max = Servo5Scroll.Maximum;

            IRC.Servo1Val = Servo1Scroll.Value;
            IRC.Servo2Val = Servo2Scroll.Value;
            IRC.Servo3Val = Servo3Scroll.Value;
            IRC.Servo4Val = Servo4Scroll.Value;
            IRC.Servo5Val = Servo5Scroll.Value;
            IRC.UsePreviousValues = cbSaveValues.Checked;

            SaveToXML<IRISConfig>(IRC, "Config.xml");
            SaveToXML<List<List<PickupPoint>>>(PickupMatrix, "ListOfPickupPoints.xml");
        }

        public void SaveToXML<T>(T serializableObject, string _FileName)
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

        private void button6_Click(object sender, EventArgs e)
        {
            int X = int.Parse(tbMatrixX.Text);
            int Y = int.Parse(tbMatrixY.Text);
            PickupPoint PO = PickupMatrix[X][Y];
            if(!cbIgnoreXY.Checked)
            {
                PO.StartX = int.Parse(tbStartX.Text);
                PO.StartY = int.Parse(tbStartY.Text);
                PO.EndX = int.Parse(tbEndX.Text);
                PO.EndY = int.Parse(tbEndY.Text);
            }
            if(string.IsNullOrWhiteSpace(tbS1Val.Text) || string.IsNullOrWhiteSpace(tbS2Val.Text) || string.IsNullOrWhiteSpace(tbS3Val.Text))
            {
                PO.Servo1Val = Servo1Scroll.Value;
                PO.Servo2Val = Servo2Scroll.Value;
                PO.Servo3Val = Servo3Scroll.Value;
            }
            else
            {
                PO.Servo1Val = int.Parse(tbS1Val.Text);
                PO.Servo2Val = int.Parse(tbS2Val.Text);
                PO.Servo3Val = int.Parse(tbS3Val.Text);
            }

            PO.IsValid = true;
        }

        private void originalImageBox_DoubleClick(object sender, EventArgs e)
        {
            if (originalImageBox.Image == null) return;
            originalImageBox.Image.Save("SourceImg.jpeg", ImageFormat.Jpeg);
            MessageBox.Show("Image Saved");
        }

        private void button7_Click(object sender, EventArgs e)
        { }

        private void btnSearchMatrix_Click(object sender, EventArgs e)
        {
            PickupPoint PP = PickupMatrix[int.Parse(tbMatrixX.Text)][int.Parse(tbMatrixY.Text)];
            tbS1Val.Text = PP.Servo1Val.ToString();
            tbS2Val.Text = PP.Servo2Val.ToString();
            tbS3Val.Text = PP.Servo3Val.ToString();
            tbEndX.Text = PP.EndX.ToString();
            tbEndY.Text = PP.EndY.ToString();
            tbStartX.Text = PP.StartX.ToString();
            tbStartY.Text = PP.StartY.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
