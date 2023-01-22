using System;
using CustomList;

namespace labs1_2_gapiyka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CustomList<string> dinosaurs = new CustomList<string>();
            dinosaurs.Handler += ListPrintCount;
            Console.WriteLine("\nCount: {0}", dinosaurs.Count);
            dinosaurs.Add("Tyrannosaurus");
            dinosaurs.Add("Amargasaurus");
            dinosaurs.Add("Mamenchisaurus");
            dinosaurs.Add("Deinonychus");
            dinosaurs.Add("Compsognathus");
            PrintAllElements(dinosaurs);

            Console.WriteLine("\nINSERT(2, COMPSOGNATHUS):");
            dinosaurs.Insert(2, "Compsognathus");
            PrintAllElements(dinosaurs);

            Console.WriteLine("\ndinosaurs[3]: {0}", dinosaurs[3]);
            Console.WriteLine("dinosaurs[9]: {0}", dinosaurs[9]);
            Console.WriteLine("dinosaurs[10]: {0}", dinosaurs[10]);

            Console.WriteLine("\nRemove(\"1\"):");
            dinosaurs.Remove(1);
            PrintAllElements(dinosaurs);


            Console.WriteLine("\nClear()");
            dinosaurs.Clear();

            Console.WriteLine("\nNEW ONE");
            CustomList<string> dinosaurs2 = new CustomList<string>(5);
            Console.WriteLine("Count: {0}", dinosaurs2.Count);

            Console.WriteLine("\nONE MORE");
            string[] lis = new string[] { "a", "bb", "cc", "kek" };
            CustomList<string> dinosaurs3 = new CustomList<string>(lis);
            Console.WriteLine("Count: {0}", dinosaurs3.Count);
            foreach (string dinosaur in dinosaurs3)
            {
                Console.WriteLine(dinosaur);
            }
            Console.WriteLine("\nContains bb: " + dinosaurs3.Contains("bb"));
            Console.WriteLine("Contains GG: " + dinosaurs3.Contains("GG"));
        }

        private static void PrintAllElements(CustomList<string> list)
        {
            foreach (string str in list)
            {
                Console.WriteLine(str);
            }
        }

        // event handler
        public static void ListPrintCount(object sender, int count)
        {
            Console.WriteLine("Count " + count);
        }
    }
}
