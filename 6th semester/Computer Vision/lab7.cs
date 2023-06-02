using System;
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
        static string file = dirOut + "car.jpg";
        static string video = dirOut + "road.mp4";
        static Point p = new Point(600, 250);
        static Size s = new Size(150, 100);
        static Rectangle rect = new Rectangle(p, s);

        static void Main(string[] args)
        {
            // 1LVL part
            Image<Bgr, byte> img = new Image<Bgr, byte>(file); //read image
            Image<Bgr, byte> segment = new Image<Bgr, byte>(s);
            Size size = GetNormalSize(img.Width, img.Height); //get comfortable size
            img.ROI = rect;
            img.CopyTo(segment); // crop segment from image
            img.ROI = Rectangle.Empty;
            UMat gray = new UMat();
            UMat grayS = new UMat();
            CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray); // cast an image to a grayscale
            CvInvoke.CvtColor(segment, grayS, ColorConversion.Bgr2Gray); // cast an segment to a grayscale
            VectorOfUMat vou = new VectorOfUMat();
            VectorOfUMat vouS = new VectorOfUMat();
            vou.Push(gray);
            vouS.Push(grayS);
            Mat fullHist = new Mat();
            Mat segmentHist = new Mat();
            Mat fullHistNorm = new Mat();
            Mat segmentHistSpread = new Mat();
            int[] channels = new int[] { 0 };
            int[] histSize = new int[] { 256 };
            float[] range = new float[] { 0, 256 };
            CvInvoke.CalcHist(vou, channels, new Mat(), fullHist, histSize, range, false); // recieve //histogram for image
            CvInvoke.CalcHist(vouS, channels, null, segmentHist, histSize, range, false); // recieve /histogram/ for segment
            
            CvInvoke.Normalize(fullHist, fullHistNorm, 0, 255, NormType.MinMax); // Normalization of /histogram
            CvInvoke.Normalize(segmentHist, segmentHistSpread, 2550, 5, NormType.L2); // streatching of //histogram
            var arrayOfHist = fullHist.GetData();
            var arrayOfHistS = segmentHist.GetData();
            var arrayOfHistNorm = fullHistNorm.GetData();
            var arrayOfHistSpread= segmentHistSpread.GetData();
            int len = histSize[0];
            int[] arr = new int[len];
            int[] arrS = new int[len];
            int[] arrNorm = new int[len];
            int[] arrSpread = new int[len];
            for (int i = 0; i < len; i++) // pesudo-rasterization
            {
                arr[i] = Convert.ToInt32((float)arrayOfHist.GetValue(i, 0));
                arrS[i] = Convert.ToInt32((float)arrayOfHistS.GetValue(i, 0));
                arrNorm[i] = Convert.ToInt32((float)arrayOfHistNorm.GetValue(i, 0));
                arrSpread[i] = Convert.ToInt32((float)arrayOfHistSpread.GetValue(i, 0));
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
            var chartS = Chart.Plot( // chart for segment of image
                new Scatter[] 
                {
                    new Scatter
                    {
                        y = arrS,
                    },
                    new Scatter
                    {
                        y = arrSpread,
                    }
                }
            );
            chart.Show();
            chartS.Show();
            
            Mat proj = new Mat();
            Mat projS = new Mat();
            CvInvoke.CalcBackProject(vou, channels, fullHistNorm, proj, range); // extrapolate updated hist /to/ image
            CvInvoke.CalcBackProject(vouS, channels, segmentHistSpread, projS, range); // extrapolate /updated /hist to segment
            //CvInvoke.Imshow("License plate", proj); //show full image
            CvInvoke.Imshow("License plate segment", projS); // show segment
            CvInvoke.WaitKey();
            CvInvoke.DestroyAllWindows();

            // 2LVL part
            VideoCapture cap = new VideoCapture(video);
            Image<Bgr, byte> frameImg;
            Mat frame = new Mat();
            
            while (true)
            {
                if (!cap.Read(frame)) break; //read frame from video stream
                frameImg = frame.ToImage<Bgr, byte>();
                Mat g = new Mat();
                CvInvoke.CvtColor(frameImg, g, ColorConversion.Bgr2Gray); // cast an image to a grayscale
                Image<Gray, byte> b = new Image<Gray, byte>(frameImg.Size);
                Image<Gray, byte> m = new Image<Gray, byte>(frameImg.Size);
                Image<Gray, byte> t = new Image<Gray, byte>(frameImg.Size);
                CvInvoke.GaussianBlur(g, b, new Size(1, 1), 0);
                CvInvoke.MorphologyEx(b, m, MorphOp.Blackhat, // morphology transforming
                    CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3),
                    Point.Empty), Point.Empty, 5, BorderType.Default, new MCvScalar(0, 0, 0)); 
                CvInvoke.Threshold(m, t, 0, 255, ThresholdType.Otsu); // thresholding by otsu's method, that
                                                                      //avoids choose a value and determines it automatically
                ////uncoment below if u want to detect countours of license plate
                //Image<Gray, byte> e = new Image<Gray, byte>(frame.Size); 
                //e = t.Canny(50, 200); //Detecting the edges of the smoothened image
                //VectorOfVectorOfPoint cnts = new VectorOfVectorOfPoint();
                ////Finding the contours from the edged image
                //CvInvoke.FindContours(e, cnts, new Mat(), RetrType.List, ChainApproxMethod.ChainApproxSimple);
                //VectorOfVectorOfPoint cntsSorted = new VectorOfVectorOfPoint();
                //string name = "";
                //for (int i = 0; i < cnts.Size; i++) // sorting contours based on the minimum area
                //{
                //    var vec = cnts[i];
                //    var d = CvInvoke.ContourArea(vec);
                //
                //    if (d>30)
                //    {
                //        var perimeter = CvInvoke.ArcLength(vec, true);
                //        VectorOfPoint approx = new VectorOfPoint();
                //        CvInvoke.ApproxPolyDP(vec, approx, perimeter * 0.4, true);
                //        if (approx.Size == 4)  // Finding the contour with four sides
                //        {
                //            cntsSorted.Push(vec);
                //            name += approx.Size + " ";
                //        }
                //    }
                //}
                ////x,y,w,h = cv2.boundingRect(c) 
                //CvInvoke.DrawContours(frame, cntsSorted, -1, new MCvScalar(0, 255, 0), 3);

                CvInvoke.Imshow("video", t);
                if (CvInvoke.WaitKey(1) == 1)
                    break;
            }
            CvInvoke.DestroyAllWindows();
        }

        static Size GetNormalSize(int w, int h)
        {
            int minW = 600;
            int minH = 400;
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