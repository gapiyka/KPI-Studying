using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;

namespace Lab
{
    class FaceRecognitionImages
    {
        public string example = Utils.dir + "examples\\2.jpg";
        MCvScalar red = new MCvScalar(0, 0, 222);
        public FaceRecognitionImages()
        {
            List<string> names = new List<string>();
            Dictionary<string, List<DlibDotNet.Matrix<float>>> personEncodings;
            personEncodings = FaceEncoding.Deserialize();
            var image = CvInvoke.Imread(example);

            var encoding = Utils.FaceEncoding(example);
            foreach(var encode in encoding)
            {
                Dictionary<string, float> counts = new Dictionary<string, float>();
                foreach (var person in personEncodings)
                {
                    float count = Utils.GetMatches(person.Value, encode);
                    if (!counts.ContainsKey(person.Key))
                        counts.Add(person.Key, count);
                    else
                        counts[person.Key] = count;
                }

                string match = "Unknown";
                float checker = 0.0f;
                foreach(var count in counts)
                {
                    if(count.Value > checker)
                    {
                        checker = count.Value;
                        match = count.Key;
                    }
                }
                names.Add(match);
            }

            var rects = Utils.FaceRects(example);
            for(int i = 0; i < rects.Length; i++)
            {
                var rect = rects[i];
                var name = "";
                if (names.Count > i) name = names[i];

                var convertedRect = new System.Drawing.Rectangle(rect.TopLeft.X, 
                    rect.TopLeft.Y, (int)rect.Width, (int)rect.Height);
                var textPoint = new System.Drawing.Point(rect.Left, rect.Top - 10);
                CvInvoke.Rectangle(image, convertedRect, red, 2) ;
                CvInvoke.PutText(image, name, textPoint,
                FontFace.HersheyScriptComplex, 1, red) ;
            }

            CvInvoke.Imshow("Face Detection", image);
            CvInvoke.WaitKey();
            CvInvoke.DestroyAllWindows();
        }
    }
}
