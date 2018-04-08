using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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

namespace IronMan
{
    public partial class ShapeDetection : Form
    {
        public ShapeDetection()
        {
            InitializeComponent();
        }

        private Image<Bgr, Byte> SourceImg;
        private Image<Gray, Byte> BlueOnlyImg;
        private Image<Gray, Byte> RedOnlyImg;
        private double BlueHueMin = 45;
        private double BlueHueMax = 145;
        private double BlueValMin = 150; //150 def
        private double BlueValMax = 255; //255

        private double RedHueMin = 110;
        private double RedHueMax = 195;
        private double RedValMin = 150;
        private double RedValMax = 255;

        public void DetectShape(bool IsBlue)
        {
            //Load the image from file and resize it for display
            //SourceImg =
            //   new Image<Bgr, byte>(fileNameTextBox.Text)
            //   .Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(SourceImg, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            Stopwatch watch = Stopwatch.StartNew();

            double cannyThreshold = 180.0;
            double cannyThresholdLinking = 120.0;
            UMat cannyEdges = new UMat();
            //CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking); ORGINAL
            if(IsBlue)
                CvInvoke.Canny(BlueOnlyImg, cannyEdges, cannyThreshold, cannyThresholdLinking);
            else
                CvInvoke.Canny(RedOnlyImg, cannyEdges, cannyThreshold, cannyThresholdLinking);

            LineSegment2D[] lines = CvInvoke.HoughLinesP(
               cannyEdges,
               1, //Distance resolution in pixel-related units
               Math.PI / 45.0, //Angle resolution measured in radians.
               20, //threshold
               30, //min Line width
               10); //gap between lines

            //watch.Stop();
            //msgBuilder.Append(String.Format("Canny & Hough lines - {0} ms; ", watch.ElapsedMilliseconds));
            //#endregion

            #region Find triangles and rectangles
            watch.Start();
            //List<Triangle2DF> triangleList = new List<Triangle2DF>();
            List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                for (int i = 0; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                        if (CvInvoke.ContourArea(approxContour, false) > 20) //u obzir uzima samo oblike cija je povrsina veca od 250
                        {
                            if (approxContour.Size == 4) //Ako oblik ima 4 ugla onda je kvadrat
                            {
                                
                                #region determine if all the angles in the contour are within [80, 100] degree
                                bool isRectangle = true;
                                Point[] pts = approxContour.ToArray();
                                LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                for (int j = 0; j < edges.Length; j++)
                                {
                                    double angle = Math.Abs(
                                       edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                    if (angle < 80 || angle > 100)
                                    {
                                        isRectangle = false;
                                        break;
                                    }
                                }
                                #endregion

                                if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                            }
                        }
                    }
                }
            }

            watch.Stop();
            label2.Text = (String.Format("Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
            #endregion


            originalImageBox.Image = SourceImg.ToBitmap(); 
            originalImageBox.Refresh();

            //#region draw triangles and rectangles
            Image<Bgr, Byte> RectangleImage = SourceImg.CopyBlank();
            foreach (RotatedRect box in boxList)
                RectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);

            if(IsBlue)
                BlueRectangleImageBox.Image = RectangleImage.ToBitmap();
            else
                RedRectangleImageBox.Image = RectangleImage.ToBitmap();
            //#endregion
        }

        public Image<Gray, byte> FilterRectangles(bool IsBlue)
        {
            // 1. Convert the image to HSV
            using (Image<Hsv, byte> hsv = SourceImg.Convert<Hsv, byte>())
            {
                // 2. Obtain the 3 channels (hue, saturation and value) that compose the HSV image
                Image<Gray, byte>[] channels = hsv.Split();

                try
                {
                    Image<Gray, Byte> imghue = channels[0];            //hsv, so channels[0] is hue.
                    Image<Gray, Byte> imgval = channels[2];            //hsv, so channels[2] is value.
                    Image<Gray, byte> Huefilter;
                    Image<Gray, byte> Valfilter;
                    if (IsBlue)
                    {
                        //filter out all but blue ...seems to be 0 to 128 ?
                        Huefilter = imghue.InRange(new Gray(BlueHueMin), new Gray(BlueHueMax));

                        //use the value channel to filter out all but brighter colors
                        Valfilter = imgval.InRange(new Gray(BlueValMin), new Gray(BlueValMax));
                    }
                    else
                    {
                        Huefilter = imghue.InRange(new Gray(RedHueMin), new Gray(RedHueMax));
                        Valfilter = imgval.InRange(new Gray(RedValMin), new Gray(RedValMax));
                    }

                    //now and the two to get the parts of the imaged that are colored and above some brightness.
                    // 3. Return filtered image
                    Image<Gray, byte> FinalImg = Huefilter.And(Valfilter);
                    return FinalImg;
                }
                finally
                {
                    channels[1].Dispose();
                    channels[2].Dispose();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DetectShape(true);
            DetectShape(false);
        }

        private void ShapeDetection_Load(object sender, EventArgs e)
        {
            Bar1.Value = Convert.ToInt16(BlueHueMin);
            Bar2.Value = Convert.ToInt16(BlueHueMax);
            Bar3.Value = Convert.ToInt16(BlueValMin);
            Bar4.Value = Convert.ToInt16(BlueValMax);
            Bar5.Value = Convert.ToInt16(RedHueMin);
            Bar6.Value = Convert.ToInt16(RedHueMax);
            Bar7.Value = Convert.ToInt16(RedValMin);
            Bar8.Value = Convert.ToInt16(RedValMax);

            label3.Text = "Min: " + Bar1.Value.ToString();
            label4.Text = "Max: " + Bar2.Value.ToString();
            label5.Text = "Min: " + Bar3.Value.ToString();
            label6.Text = "Max: " + Bar4.Value.ToString();
            label7.Text = "Min: " + Bar5.Value.ToString();
            label8.Text = "Max: " + Bar6.Value.ToString();
            label9.Text = "Min: " + Bar7.Value.ToString();
            label10.Text = "Max: " + Bar8.Value.ToString();

            SourceImg = new Image<Bgr, byte>(fileNameTextBox.Text)
               .Resize(855, 594, Emgu.CV.CvEnum.Inter.Linear, true);
            SetBlueSliders(sender, e);
            SetRedSliders(sender, e);
        }

        private void SetBlueSliders(object sender, EventArgs e)
        {
            BlueHueMin = Bar1.Value;
            BlueHueMax = Bar2.Value;
            BlueValMin = Bar3.Value;
            BlueValMax = Bar4.Value;
            label3.Text = "Min: " + BlueHueMin.ToString();
            label4.Text = "Max: " + BlueHueMax.ToString();
            label5.Text = "Min: " + BlueValMin.ToString();
            label6.Text = "Max: " + BlueValMax.ToString();
            BlueOnlyImg = FilterRectangles(true);
            BlueRectangleImageBox.Image = BlueOnlyImg.ToBitmap();
        }

        private void SetRedSliders(object sender, EventArgs e)
        {
            RedHueMin = Bar5.Value;
            RedHueMax = Bar6.Value;
            RedValMin = Bar7.Value;
            RedValMax = Bar8.Value;
            label7.Text = "Min: " + RedHueMin.ToString();
            label8.Text = "Max: " + RedHueMax.ToString();
            label9.Text = "Min: " + RedValMin.ToString();
            label10.Text = "Max: " + RedValMax.ToString();
            RedOnlyImg = FilterRectangles(false);
            RedRectangleImageBox.Image = RedOnlyImg.ToBitmap();
        }
    }
}
