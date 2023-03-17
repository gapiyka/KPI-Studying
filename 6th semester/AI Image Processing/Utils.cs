using System;
using System.Collections.Generic;
using DlibDotNet;
using DlibDotNet.Dnn;
using System.IO;
using Numpy;

namespace Lab
{
    class Utils
    {
        public static string dir = "..\\..\\..\\src\\";
        public static string rootDir = dir + "dataset\\";
        public static string models = dir + "models\\";
        static string fe = models + "dlib_face_recognition_resnet_model_v1.dat";
        static string sp = models + "shape_predictor_68_face_landmarks.dat";
        public static FrontalFaceDetector faceDetector = Dlib.GetFrontalFaceDetector();
        // use a face landmarking model to align faces to a standard pose
        public static ShapePredictor shapePredictor = ShapePredictor.Deserialize(sp);
        // load the DNN responsible for face recognition.
        public static LossMetric faceEncoder = LossMetric.Deserialize(fe);
        static List<string> validExtensions = new List<string> {"png", "jpg", "jpeg", "bmp" };

        public static List<string> GetImagePaths(string[] classNames)
        {
            List<string> imagePaths = new List<string>();
            foreach (var className in classNames)
            {
                var classDir = rootDir + className;
                string[] classFilePaths = Directory.GetFiles(classDir);
                foreach (var filePath in classFilePaths)
                {
                    var splits = filePath.Split('.');
                    string ext = splits[splits.Length - 1].ToLower();
                    if (!validExtensions.Contains(ext))
                    {
                        Console.WriteLine("Skipping file: " + filePath);
                        continue;
                    }
                    imagePaths.Add(filePath);
                }
            }
            
            return imagePaths;
        }

        public static float GetMatches(List<Matrix<float>> known, Matrix<float> unknown)
        {
            float threshold = 0.6f;
            float sum = 0.0f;
            foreach(var k in known)
            {
                var n1 = np.array(k.ToArray());
                var n2 = np.array(unknown.ToArray());
                var dif = n2 - n1;
                var distance = float.Parse(np.linalg.norm(dif).repr); 
                if (distance <= threshold)
                    sum += distance;
            }
            
            return sum;
        }

        public static List<Matrix<float>> FaceEncoding(string image)
        {
            var img = Dlib.LoadImageAsMatrix<RgbPixel>(image);
            List<Matrix<float>> list = new List<Matrix<float>>();
            foreach (var landmark in FaceLandmarks(img))
            {
                var face = faceEncoder.Operator(landmark);
                list.Add(face[0]);
            }
            if (list.Count == 0) 
                Console.WriteLine("Faces not found");
            return list;
            
        }

        public static List<Matrix<RgbPixel>> FaceLandmarks(Matrix<RgbPixel> image)
        {
            List<Matrix<RgbPixel>> list = new List<Matrix<RgbPixel>>();
            foreach(var rect in FaceRects(image))
            {
                var shape = shapePredictor.Detect(image, rect);
                var faceChipDetail = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                var faceChip = Dlib.ExtractImageChip<RgbPixel>(image, faceChipDetail);
                list.Add(faceChip);
            }
            return list;
        }

        public static Rectangle[] FaceRects(string image)
        {
            var img = Dlib.LoadImageAsMatrix<RgbPixel>(image);
            return FaceRects(img);
        }

        public static Rectangle[] FaceRects(Matrix<RgbPixel> image)
        {
            return faceDetector.Operator(image);
        }
    }
}
