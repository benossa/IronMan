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

        public void DetectShape()
        {
            //Load the image from file and resize it for display
            Image<Bgr, Byte> img =
               new Image<Bgr, byte>(fileNameTextBox.Text)
               .Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);

            //Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();
            Stopwatch watch = Stopwatch.StartNew();

            //#region circle detection
            //double circleAccumulatorThreshold = 120;
            //CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);

            //watch.Stop();
            //msgBuilder.Append(String.Format("Hough circles - {0} ms; ", watch.ElapsedMilliseconds));
            //#endregion

            //#region Canny and edge detection
            //watch.Reset(); watch.Start();
            double cannyThreshold = 180.0;
            double cannyThresholdLinking = 120.0;
            UMat cannyEdges = new UMat();
            CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);

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


            originalImageBox.Image = img.ToBitmap(); 
            originalImageBox.Refresh();

            //#region draw triangles and rectangles
            Image<Bgr, Byte> BlueRectangleImage = img.CopyBlank();
            foreach (RotatedRect box in boxList)
                BlueRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);

            BlueRectangleImageBox.Image = BlueRectangleImage.ToBitmap();
            BlueRectangleImageBox.Refresh();
            //#endregion
        }

        public void DetectBlue()
        {
            Image<Bgr, Byte> img =
               new Image<Bgr, byte>(fileNameTextBox.Text)
               .Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            // 1. Convert the image to HSV
            using (Image<Hsv, byte> hsv = img.Convert<Hsv, byte>())
            {
                // 2. Obtain the 3 channels (hue, saturation and value) that compose the HSV image
                Image<Gray, byte>[] channels = hsv.Split();

                try
                {
                    // 3. Remove all pixels from the hue channel that are not in the range [40, 60]
                    //CvInvoke.cvInRangeS(channels[0], new Gray(40).MCvScalar, new Gray(60).MCvScalar, channels[0]);
                    //CvInvoke.CvtColor(channels[0], channels[0], new Gray(40).MCvScalar, new Gray(60).MCvScalar);
                    //channels[0].ThresholdToZero()
                    //channels[0].
                    // 4. Display the result
                    RedRectangleImageBox.Image = channels[0].ToBitmap();
                    MessageBox.Show("");
                    RedRectangleImageBox.Image = channels[1].ToBitmap();
                    MessageBox.Show("");
                    RedRectangleImageBox.Image = channels[2].ToBitmap();
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
            DetectShape();
        }
    }
}
