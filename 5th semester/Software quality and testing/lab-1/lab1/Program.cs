using System;

namespace lab1
{
    class Program
    {
        static void InputToParams(string file, ref int height, ref int width, ref int gens, ref string matrix)
        {
            string[] lines = System.IO.File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0) gens = Int32.Parse(lines[i]);
                else if (i == 1)
                {
                    string[] size = lines[i].Split(" ");
                    height = Int32.Parse(size[1]);
                    width = Int32.Parse(size[0]);
                }
                else
                {
                    matrix += (lines[i] + "\n");
                }
            }
        }

        static void Main(string[] args)
        {
            string file = (args.Length == 1) ? args[0] : "input.txt";
            int height = 0;
            int width = 0;
            int gens = 0;
            string matrix = "";
            InputToParams(file, ref height, ref width, ref gens, ref matrix);
            CGoL cGoL = (matrix == "") ? new CGoL() : new CGoL(height, width, gens, matrix);
            //CGoL cGoL = new CGoL(5,5,5, "xx...\nxx...\n.....\n.....\n.....");
            cGoL.DrawGrid(gens);
            System.IO.File.WriteAllText("output.txt", cGoL.GridToString());
        }
    }
}
