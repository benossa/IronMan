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
        public PickupObject RobotPosition;
        private bool TimerInUse = false;
        private System.Timers.Timer timer;

        #region Image Settings
        private int ImageWidth = 800;
        private int ImageHeight = 600;

        private double Type1HueMin = 0;  //80;
        private double Type1HueMax = 255; //145;
        private double Type1ValMin = 83; //150 def
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

        public void DetectRectangles(bool Type1)
        {
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
                                    obj.CenterX = tempRect.Center.X;
                                    obj.CenterY = tempRect.Center.Y;
                                    obj.Size = approxContourSize;
                                    obj.Type = Type1 ? "Type 1" : "Type 2";
                                    obj.Angle = tempRect.Angle;
                                    PickupObjects.Add(obj);

                                    tbCoordinates.Text += $"Type: {obj.Type}" + Environment.NewLine;
                                    tbCoordinates.Text += $"Size: {obj.Size}" + Environment.NewLine;
                                    tbCoordinates.Text += $"Angle: {obj.Angle}" + Environment.NewLine;
                                    tbCoordinates.Text += $"Center: {obj.CenterX}, {obj.CenterY}" + Environment.NewLine;
                                    tbCoordinates.Text += "------------------------------" + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            //prikazemo oblik gdje je smjestena baza robota
            SourceImg.Draw(new Rectangle(int.Parse(RobotPosX.Text), int.Parse(RobotPosY.Text), int.Parse(RobotPosWidth.Text), int.Parse(RobotPosHeight.Text)), new Bgr(Color.Yellow), 4);
            RobotPosition = new PickupObject() { CenterX = 0};

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
            //ovdje ide parsiranje slike sa kamere u SourceImg
            SourceImg = CamCapture.QueryFrame().ToImage<Bgr, Byte>();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);
            DetectRectangles(true);
            DetectRectangles(false);
            watch.Stop();

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
            
            GetCamerasList();
            SetType1Sliders(sender, e);
            SetType2Sliders(sender, e);

            //timer kao automatski okidac za uzimanje uzorka slike
            timer = new System.Timers.Timer(400);
            timer.SynchronizingObject = this;
            timer.Elapsed += HandleTimerElapsed;
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
    }
}
