using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using System.IO;
using Emgu.CV.Util;
using System.Linq;

namespace CompVision
{
    class Program
    {
        static string dirOut = "..\\..\\..\\..\\";
        static string video = dirOut + "video.mp4";
        static string names = dirOut + "coco.names";
        static string cfg = dirOut + "yolov3-tiny.cfg";
        static string weights = dirOut + "yolov3-tiny.weights";

        static void Main(string[] args)
        {
            var vc = new VideoCapture(video);
            Mat frame = new Mat();
            while (true)
            {
                if (!vc.Read(frame)) break; //read frame
                Mat sepia = new Mat();
                var kernel = new Matrix<float>(new float[3, 3] {
                    { 0.272f, 0.534f, 0.131f },
                    { 0.349f, 0.686f, 0.168f },
                    { 0.393f, 0.769f, 0.189f } });
                CvInvoke.Transform(frame, sepia, kernel); //transfrom to sepia
                Mat upgrade = new Mat();
                CvInvoke.CvtColor(sepia, upgrade, ColorConversion.Bgr2Gray);//cast to gray
                CvInvoke.EqualizeHist(upgrade, upgrade);//equalize
                CvInvoke.ConvertScaleAbs(upgrade, upgrade, 1, 0);//improve contrast

                //object detection:
                var classLabels = File.ReadAllLines(names);
                var net = DnnInvoke.ReadNetFromDarknet(cfg, weights);
                net.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
                net.SetPreferableTarget(Target.Cpu);
                var boxes = new VectorOfRect();
                var scores = new VectorOfFloat();
                var indices = new VectorOfInt();
                var output = new VectorOfMat();
                Mat resized = new Mat();
                CvInvoke.Resize(frame, resized, new System.Drawing.Size(1920, 1084));
                var input = DnnInvoke.BlobFromImage(resized, 1 / 255.0, swapRB: true);
                net.SetInput(input);
                net.Forward(output, net.UnconnectedOutLayersNames);

                for (int i = 0; i < output.Size; i++)
                {
                    var mat = output[i];//output[1];
                    var data = (float[,])mat.GetData();

                    for (int j = 0; j < data.GetLength(0); j++)
                    {
                        float[] row = Enumerable.Range(0, data.GetLength(1))
                                      .Select(x => data[j, x])
                                      .ToArray();

                        var rowScore = row.Skip(5).ToArray();
                        var classId = rowScore.ToList().IndexOf(rowScore.Max());
                        var confidence = rowScore[classId];

                        //if (confidence > 0.5f) // change for wanted percent
                        {
                            var centerX = (int)(row[0] * resized.Width);
                            var centerY = (int)(row[1] * resized.Height);
                            var boxWidth = (int)(row[2] * resized.Width);
                            var boxHeight = (int)(row[3] * resized.Height);

                            var x = (int)(centerX - (boxWidth / 2));
                            var y = (int)(centerY - (boxHeight / 2));

                            boxes.Push(new System.Drawing.Rectangle[] { new System.Drawing.Rectangle(x, y, boxWidth, boxHeight) });
                            indices.Push(new int[] { classId });
                            scores.Push(new float[] { confidence });
                        }
                    }
                }

                var bestIndex = DnnInvoke.NMSBoxes(boxes.ToArray(), scores.ToArray(), 0.4f, 0.6f);

                for (int i = 0; i < bestIndex.Length; i++)
                {
                    int index = bestIndex[i];
                    var box = boxes[index];
                    string text = classLabels[indices[index]] + ", " + (int)(scores[index] * 100) + "%";
                    CvInvoke.Rectangle(resized, box, new MCvScalar(0, 255, 0));
                    CvInvoke.PutText(resized, text, new System.Drawing.Point(box.X, box.Y - 5),
                    FontFace.HersheyPlain, 1.0, new MCvScalar(0, 0, 180));
                }

                CvInvoke.Imshow("Image", resized); //show image

                if (CvInvoke.WaitKey(1) == 1)
                    break;
            }
            CvInvoke.WaitKey();
            CvInvoke.DestroyAllWindows();
        }
    }
}