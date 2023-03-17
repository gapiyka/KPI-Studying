using DlibDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab
{
    class FaceEncoding
    {
        static string save = Utils.dir + "NotPickle.dat";
        static void Serealize(Dictionary<string, List<Matrix<float>>> encodings)
        {
            Dictionary<string, List<float[]>> converted = new Dictionary<string, List<float[]>>();
            foreach(var encode in encodings)
            {
                List<float[]> list = new List<float[]>();
                foreach(var row in encode.Value)
                    list.Add(row.ToArray());
                converted.Add(encode.Key, list);
            }
            FileStream fs = new FileStream(save, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, converted);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public static Dictionary<string, List<Matrix<float>>> Deserialize()
        {
            Dictionary<string, List<Matrix<float>>> converted = new Dictionary<string, List<Matrix<float>>>();
            Dictionary<string, List<float[]>> encodings = new Dictionary<string, List<float[]>>();
            FileStream fs = new FileStream(save, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                encodings = (Dictionary<string, List<float[]>>)formatter.Deserialize(fs);
                foreach (var encode in encodings)
                {
                    List<Matrix<float>> list = new List<Matrix<float>>();
                    foreach (var row in encode.Value)
                    {
                        var newOne = new Matrix<float>(row, 1, 128);
                        list.Add(newOne);
                    }
                        
                    converted.Add(encode.Key, list);
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return converted;
        }

        public FaceEncoding()
        {
            DirectoryInfo[] directories = new DirectoryInfo(Utils.rootDir).GetDirectories();
            string[] dirs = Array.ConvertAll(directories, item => item.Name);
            List<string> imagePaths = Utils.GetImagePaths(dirs);
            Dictionary<string, List<Matrix<float>>> nameEncodingsDict = new Dictionary<string, List<Matrix<float>>>();
            int currentImage = 1;
            foreach(var image in imagePaths)
            {
                Console.WriteLine("Image Processed: " + currentImage++ + "/" + imagePaths.Count);
                var encoding = Utils.FaceEncoding(image);
                string[] splits = image.Split("\\");
                string name = splits[splits.Length-2];
                List<Matrix<float>> e = new List<Matrix<float>>();
                if (!nameEncodingsDict.ContainsKey(name))
                    nameEncodingsDict.Add(name, e);
                else
                    e = nameEncodingsDict[name];

                e.AddRange(encoding);
                nameEncodingsDict[name] = e;
            }
            Serealize(nameEncodingsDict);
        }
    }
}
