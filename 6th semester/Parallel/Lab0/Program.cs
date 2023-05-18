//-------------------------------------
//--Лабораторна робота ЛР0 Варіант 4
//-- F1: C = A + SORT(B) *(MA*MЕ)
//-- F2: MG = MAX(MH) *(MK*ML)
//-- F3: O = SORT(P)*SORT(MR*MS)
//-- Гапій Д.   ІП-05
//-- Дата  12 02 2023
//-------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;


class Data
{
    public const int size = 2; // change this for vector / matrix size
    public enum MathType // value types for operations
    {
        Vector,
        Matrix,
        Scalar
    }
}
namespace WinFormsApp1
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.Name = "Thread_Main";
            // creating of new threads
            Thread T1 = new Thread(F1) { Name = "Thread_T1" };
            Thread T2 = new Thread(F2) { Name = "Thread_T2" };
            Thread T3 = new Thread(F3) { Name = "Thread_T3" };
            T1.Start();
            T2.Start();
            T3.Start();
        }


        // RECIEVE INPUT VALUES
        static List<int[][]> ParseInputs(Dictionary<string, Data.MathType> dict, string[] arr)
        {
            List<int[][]> result = new List<int[][]>();
            for(int i = 0; i < dict.Count; i++)
            {
                var item = dict.ElementAt(i);
                int iterations = (item.Value == Data.MathType.Matrix) ? Data.size : 1;
                int vSize = (item.Value == Data.MathType.Scalar) ? 1 : Data.size;
                int[][] tempMatrix = new int[iterations][];

                string input = arr[i];
                string[] lines = input.Split('\n');
                for (int j = 0; j < iterations; j++)
                {
                    string[] line = lines[j].Split(' ');
                    int[] vector = new int[vSize];
                    for (int num = 0; num < vSize; num++)
                        vector[num] = Int32.Parse(line[num]);
                    tempMatrix[j] = vector;
                    if (j == iterations - 1)
                        result.Add(tempMatrix);
                }
            }
            return result;
        }

        // OUTPUT RESULT
        static void Output(int[][] result, string value)
        {
            string answer = "";
            answer += (value + " equal:\n");
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                    answer += (result[i][j] + " ");
                answer += "\n";
            }
            var form = new AnswerForm(answer);
            form.ShowDialog();
        }

        // MULTIPLY MATRICES
        static int[][] MatrixMultiplication(int[][] m1, int[][] m2)
        {
            int h = m1.Length;
            int w = m2.Length;
            int[][] m3 = new int[h][];
            for (int i = 0; i < h; i++)
            {
                m3[i] = new int[w];
                for (int j = 0; j < w; j++)
                    for (int k = 0; k < w; k++)
                        m3[i][j] += m1[i][k] * m2[k][j];
            }
            return m3;
        }

        // MULTIPLY MATRIX WITH SCALAR
        static int[][] MatrixMultiplication(int[][] m, int k)
        {
            int s = m.Length;
            int s2 = m[0].Length;
            for (int i = 0; i < s; i++)
                for (int j = 0; j < s2; j++)
                    m[i][j] = m[i][j] * k;

            return m;
        }

        // SUM 2 MATRICES
        static int[][] MatrixAddition(int[][] m1, int[][] m2)
        {
            int s = m1.Length;
            int s2 = m1[0].Length;
            int[][] m3 = new int[s][];
            for (int i = 0; i < s; i++)
            {
                m3[i] = new int[s2];
                for (int j = 0; j < s2; j++)
                    m3[i][j] = m1[i][j] + m2[i][j];
            }
            return m3;
        }

        // SORT MATRIX / VECTOR
        static int[][] MatrixSort(int[][] m)
        {
            int s = m.Length;
            int s2 = m[0].Length;
            int[] arr = new int[s * s2];
            int i, j, counter = 0;
            for (i = 0; i < s; i++)
                for (j = 0; j < s2; j++)
                    arr[counter++] = m[i][j];

            Array.Sort(arr);
            counter = 0;
            for (i = 0; i < s; i++)
                for (j = 0; j < s2; j++)
                    m[i][j] = arr[counter++];

            return m;
        }

        // RECIEVE MAX VALUE IN MATRIX
        static int[][] MatrixMax(int[][] m)
        {
            int s = m.Length;
            int s2 = m[0].Length;
            int i, j, max = 0;
            for (i = 0; i < s; i++)
                for (j = 0; j < s2; j++)
                    if (max < m[i][j]) max = m[i][j];

            int[][] maxM = new int[1][] { new int[1] { max } };
            return maxM;
        }

        static string[] FillFileNumbers(Dictionary<string, Data.MathType> dict, string path)
        {
            string context = File.ReadAllText(path);
            return context.Split("x");
        }

        static string[] FillByNumbers(Dictionary<string, Data.MathType> dict, Func<int> func)
        {
            string[] answer = new string[dict.Count];
            for (int i = 0; i < dict.Count; i++)
            {
                var item = dict.ElementAt(i);
                int iterations = (item.Value == Data.MathType.Matrix) ? Data.size : 1;
                int vSize = (item.Value == Data.MathType.Scalar) ? 1 : Data.size;
                string temp = "";
                for (int j = 0; j < iterations; j++)
                {
                    for (int n = 0; n < vSize; n++)
                        temp += (func() + " ");
                    temp += "\n";
                    if (j == iterations - 1)
                        answer[i] = temp;
                }
            }
            return answer;
        }

        static string[] FillDeclaredNumber(Dictionary<string, Data.MathType> dict, int num)
        {
            return FillByNumbers(dict, () => { return num; });
        }

        static string[] FillRandomNumbers(Dictionary<string, Data.MathType> dict, int min, int max)
        {
            return FillByNumbers(dict, () => { 
                Random random = new Random(DateTime.Now.Millisecond); 
                return random.Next(min, max); 
            });
        }

        // func1
        static void F1()
        {
            Dictionary<string, Data.MathType> dict = new Dictionary<string, Data.MathType>()
            {
                { "A", Data.MathType.Vector },
                { "B", Data.MathType.Vector },
                { "MA", Data.MathType.Matrix },
                { "ME", Data.MathType.Matrix }
            };
            //input
            List<int[][]> inputs;
            string[] inputArr;
            if (Data.size > 1000)
                inputArr = FillFileNumbers(dict, "./text.txt");
            else
            {
                var form = new Form1();
                var formResult = form.ShowDialog();
                inputArr = new string[] { form.A, form.B, form.MA, form.ME };
            }
            inputs = ParseInputs(dict, inputArr);
            //C = A + SORT(B)*(MA*MЕ)
            int[][] o1 = MatrixMultiplication(inputs[2], inputs[3]);
            int[][] o2 = MatrixSort(inputs[1]);
            int[][] o3 = MatrixMultiplication(o2, o1);
            int[][] o4 = MatrixAddition(inputs[0], o3);
            Output(o4, "C");
        }

        // func2
        static void F2()
        {
            Dictionary<string, Data.MathType> dict = new Dictionary<string, Data.MathType>()
            {
                { "MH", Data.MathType.Matrix },
                { "MK", Data.MathType.Matrix },
                { "ML", Data.MathType.Matrix }
            };
            //input
            List<int[][]> inputs;
            string[] inputArr;
            if (Data.size > 1000)
                inputArr = FillDeclaredNumber(dict, 1);
            else
            {
                var form = new Form2();
                var formResult = form.ShowDialog();
                inputArr = new string[] { form.MH, form.MK, form.ML };
            }
            inputs = ParseInputs(dict, inputArr);
            //MG = MAX(MH) *(MK*ML)
            int[][] o1 = MatrixMultiplication(inputs[1], inputs[2]);
            int[][] o2 = MatrixMax(inputs[0]);
            int[][] o3 = MatrixMultiplication(o1, o2[0][0]);
            Output(o3, "MG");
        }

        // func3
        static void F3()
        {
            Dictionary<string, Data.MathType> dict = new Dictionary<string, Data.MathType>()
            {
                { "P", Data.MathType.Vector },
                { "MR", Data.MathType.Matrix },
                { "MS", Data.MathType.Matrix }
            };
            //input
            List<int[][]> inputs;
            string[] inputArr;
            if (Data.size > 1000)
                inputArr = FillRandomNumbers(dict, 1, 3);
            else
            {
                var form = new Form3();
                var formResult = form.ShowDialog();
                inputArr = new string[] { form.P, form.MR, form.MS };
            }
            inputs = ParseInputs(dict, inputArr);
            //O = SORT(P) * SORT(MR * MS)
            int[][] o1 = MatrixSort(inputs[0]);
            int[][] o2 = MatrixMultiplication(inputs[1], inputs[2]);
            int[][] o3 = MatrixSort(o2);
            int[][] o4 = MatrixMultiplication(o1, o3);
            Output(o4, "O");
        }
    }
}
