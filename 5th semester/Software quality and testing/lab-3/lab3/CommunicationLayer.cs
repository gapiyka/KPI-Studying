using System;

namespace lab3
{
    public class CommunicationLayer
    {
        public IOManager ioManager;
        public LogicManager logicManager;
        public string inputFile;
        public string inputResult;
        public string outputResult;
        public int height = 0;
        public int width = 0;
        public char[,] grid;

        public CommunicationLayer(IOManager io, LogicManager logic, string[] args)
        {
            ioManager = io;
            logicManager = logic;
            RunApp(args);
        }

        public void RunApp(string[] args)
        {
            inputFile = (args.Length > 0) ? args[0] : "";
            inputResult = ioManager.ParseFile(inputFile);
            if (inputResult == Constants.ER_FILE_NOT_EXIST) 
                throw new Exception(inputResult);
            grid = CalculateParams(ref height, ref width, inputResult);
            grid = logicManager.ExecRound(grid);
            outputResult = GridToText(height, width, grid);
            ioManager.GiveAnswer(outputResult);
        }

        public char[,] CalculateParams(ref int height, ref int width, string str)
        {
            char curr = '\0';
            string[] lines = str.Split("\n");
            string[] sizeArgs = lines[0].Split(" ");
            if (sizeArgs.Length != 2 || lines.Length < 2) 
                throw new Exception(Constants.ER_PARAMS_WRONG);
            height = Int32.Parse(sizeArgs[0]);
            width = Int32.Parse(sizeArgs[1]);
            if (height < 1 || width < 1 || height < lines.Length - 1 || width < lines[1].Length - 1) 
                throw new Exception(Constants.ER_PARAMS_WRONG);
            grid = new char[height, width];
            for(int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    curr = lines[row + 1][column];
                    grid[row, column] = (curr == '#' || curr == 'p') ? curr : '.';
                }
            }
            return grid;
        }

        public string GridToText(int height, int width, char[,] grid)
        {
            string res = height + " " + width + "\n";
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    res += grid[row, column];
                }
                res += "\n";
            }
            return res;
        }
    }
}
