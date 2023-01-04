using System;
using System.IO;

namespace lab3
{
    public class CFSManager : IOManager
    {
        public override string ParseFile(string file)
        {
            if (file == "") file = AskFileName();
            return (File.Exists(file)) ? File.ReadAllText(file) : Constants.ER_FILE_NOT_EXIST;
        }

        public override string AskFileName()
        {
            Console.WriteLine("Please, input file path / name: ");
            return Console.ReadLine();
        }

        public override void GiveAnswer(string text)
        {
            Console.WriteLine(text);
            File.WriteAllText("output.txt", text);
        }
    }
}
