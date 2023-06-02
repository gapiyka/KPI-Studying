using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using XPlot.Plotly;

namespace CompVision
{
    class Program
    {
        static MCvScalar white = new MCvScalar(255, 255, 255);
        static MCvScalar red = new MCvScalar(0, 0, 222);
        static string dirOut = "..\\..\\..\\..\\";
        static string file_standard = dirOut + "google2.jpg";
        static string file_operative = dirOut + "oper-new-rgb.jpg";

        static void Main(string[] args)
        {
            #region Clusterization with Kmeans
            Bgr[] clusterColors = new Bgr[] {
                new Bgr(0,0,255),
                new Bgr(0, 255, 0),
                new Bgr(255, 100, 100),
                new Bgr(255,0,255),
                new Bgr(133,0,99),
                new Bgr(130,12,49),
                new Bgr(0, 255, 255)};
            
            Image<Bgr, float> src = new Image<Bgr, float>(file_operative);
            Matrix<float> samples = new Matrix<float>(src.Rows * src.Cols, 1, 3);
            Matrix<int> finalClusters = new Matrix<int>(src.Rows * src.Cols, 1);
            
            for (int y = 0; y < src.Rows; y++)
            {
                for (int x = 0; x < src.Cols; x++)
                {
                    samples.Data[y + x * src.Rows, 0] = (float)src[y, x].Blue;
                    samples.Data[y + x * src.Rows, 1] = (float)src[y, x].Green;
                    samples.Data[y + x * src.Rows, 2] = (float)src[y, x].Red;
                }
            }
            
            MCvTermCriteria term = new MCvTermCriteria(100, 0.5);
            term.Type = TermCritType.Iter | TermCritType.Eps;
            
            int clusterCount = 3;
            int attempts = 10;
            Matrix<Single> centers = new Matrix<Single>(clusterCount, src.Rows * src.Cols);
            CvInvoke.Kmeans(samples, clusterCount, finalClusters, term, attempts, KMeansInitType.PPCenters);
            
            Image<Bgr, float> new_image = new Image<Bgr, float>(src.Size);
            
            for (int y = 0; y < src.Rows; y++)
                for (int x = 0; x < src.Cols; x++)
                {
                    PointF p = new PointF(x, y);
                    new_image.Draw(new CircleF(p, 1.0f), clusterColors[finalClusters[y + x * src.Rows, 0]], 1);
                }
            
            CvInvoke.Imshow("clustered image", new_image);
            CvInvoke.WaitKey(0);
            #endregion

            Image<Bgr, byte> img = new Image<Bgr, byte>(file_operative); //read image
            Mat gray = new Mat();
            CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray); // cast an image to a grayscale
            Mat clahe = new Mat();
            CvInvoke.CLAHE(gray, 5, new Size(8, 8), clahe); // contrast limited adaptive histogram equalization
            //CvInvoke.EqualizeHist(gray, clahe); // histogram equalization
            Image<Bgr, byte> img2 = new Image<Bgr, byte>(file_standard); //read image
            Mat gray2 = new Mat();
            CvInvoke.CvtColor(img2, gray2, ColorConversion.Bgr2Gray); // cast an image to a grayscale
            Mat clahe2 = new Mat();
            //CvInvoke.CLAHE(gray, 5, new Size(8, 8), clahe); // contrast limited adaptive histogram equalization
            CvInvoke.EqualizeHist(gray2, clahe2); // histogram equalization

            #region Histograms
            VectorOfMat vou = new VectorOfMat();
            VectorOfMat vou2 = new VectorOfMat();
            vou.Push(gray);
            vou2.Push(clahe);
            Mat fullHist = new Mat();
            Mat fullHistNorm = new Mat();
            int[] channels = new int[] { 0 };
            int[] histSize = new int[] { 256 };
            float[] range = new float[] { 0, 256 };
            CvInvoke.CalcHist(vou, channels, new Mat(), fullHist, histSize, range, false); 
            CvInvoke.CalcHist(vou2, channels, new Mat(), fullHistNorm, histSize, range, false);
            var arrayOfHist = fullHist.GetData();
            var arrayOfHistNorm = fullHistNorm.GetData();
            int len = histSize[0];
            int[] arr = new int[len];
            int[] arrNorm = new int[len];
            for (int i = 0; i < len; i++) // pesudo-rasterization
            {
                arr[i] = Convert.ToInt32((float)arrayOfHist.GetValue(i, 0));
                arrNorm[i] = Convert.ToInt32((float)arrayOfHistNorm.GetValue(i, 0));
            }
            
            var chart = Chart.Plot( // chart for full image
                new Scatter[]
                {
                    new Scatter
                    {
                        y = arr,
                    },
                    new Scatter
                    {
                        y = arrNorm,
                    },
                }
            );
            chart.Show();
            #endregion

            Mat blur = new Mat();
            CvInvoke.GaussianBlur(clahe, blur, new Size(1, 1), 0); // 1 - oper || 9 - stnrd
            Mat blur2 = new Mat();
            CvInvoke.GaussianBlur(clahe2, blur2, new Size(9, 9), 0); // 1 - oper || 9 - stnrd

            Mat thresh = new Mat();
            CvInvoke.Threshold(clahe, thresh, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu); // blur for standard
            Mat morph = new Mat();
            CvInvoke.MorphologyEx(thresh, morph, MorphOp.Open, CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(5, 5),
                    Point.Empty), Point.Empty, 2, BorderType.Default, new MCvScalar(0, 0, 0));
            Mat canny = new Mat();
            Mat canny2 = new Mat();

            CvInvoke.Canny(blur, canny, 10, 300);
            CvInvoke.Canny(blur2, canny2, 10, 300);
            Image<Gray, byte> result = new Image<Gray, byte>(gray.Width * 2, gray.Height);
            result.ROI = new Rectangle(0, 0, gray.Width, gray.Height);
            gray.CopyTo(result);
            result.ROI = Rectangle.Empty;
            result.ROI = new Rectangle(gray.Width, 0, gray2.Width, gray2.Height);
            gray2.CopyTo(result);
            result.ROI = Rectangle.Empty;
            Size size = GetNormalSize(result.Width, result.Height); //get comfortable size
            CircleF[] circles = CvInvoke.HoughCircles(canny, HoughModes.Gradient, 1, 30, 100, 11, 1, 30);
            CircleF[] circles2 = CvInvoke.HoughCircles(canny2, HoughModes.Gradient, 1, 30, 100, 11, 1, 30);
            List<Point> points = new List<Point>();
            foreach(var circle in circles)
            {
                Point p = new Point((int)circle.Center.X, (int)circle.Center.Y);
                points.Add(p);
                CvInvoke.Circle(gray, p, (int)circle.Radius, red, 3);
            }
            foreach(var circle in circles2)
            {
                Point p = new Point((int)circle.Center.X, (int)circle.Center.Y);
                int pad = 10;
                points.ForEach(point => // improvise MatchDescriptor
                {
                    if ((point.X >= p.X - pad && point.X <= p.X + pad) &&
                        (point.Y >= p.Y - pad && point.Y <= p.Y + pad))
                    {
                        Point newP = new Point(point.X + gray.Width, point.Y);
                        CvInvoke.Line(result, p, newP, white, 3);
                    }
                        
                });
                
                CvInvoke.Circle(gray2, p, (int)circle.Radius, red, 3);
            }

            CvInvoke.Resize(result, result, size);
            CvInvoke.Imshow("Map", result); //show full image
            CvInvoke.WaitKey();
            //CvInvoke.Imshow("Map normalized", canny); //show normalized image
            //CvInvoke.WaitKey();
            CvInvoke.DestroyAllWindows();
        }

        static Size GetNormalSize(int w, int h)
        {
            int minW = 1000;
            int minH = 500;
            Size newSize = new Size();
            double k = (double)w / h;
            if (w < h)
            {
                newSize.Width = (w < minW) ? w : minW;
                newSize.Height = (int)(newSize.Width / k);
            }
            else
            {
                newSize.Height = (h < minH) ? h : minH;
                newSize.Width = (int)(newSize.Height * k);
            }
            return newSize;
        }
    }
}