using System;
using System.Collections.Generic;
using System.Text;

namespace lab1
{
    public class CGoL
    {
        private int height;
        private int width;
        private bool[,] startGrid;
        private bool[,] resultGrid;
        public int Height { get { return height; } }
        public int Width { get { return width; } }

        public CGoL(int height = 5, int width = 5, int gens = 1, string matrix = ".....\n..x..\n..x..\n..x..\n.....")
        {
            this.height = height;
            this.width = width;
            startGrid = StringToMatrix(matrix);
            resultGrid = GenResult(startGrid, gens);
        }

        public bool[,] StringToMatrix(string str)
        {
            string[] rows = str.Split("\n");
            bool[,] matrix = new bool[height, width];
            for (int i = 0; i < height; i++)
            {
                string row = rows[i];
                for (int j = 0; j < width; j++)
                {
                    matrix[i, j] = (row[j].ToString() == "x") ? true : false;
                }
            }
            return matrix;
        }

        public int GetNeighbors(bool[,] grid, int x, int y)
        {
            int NumOfAliveNeighbors = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                
                    int column = j;
                    int row = i;
                    if (i == x && j == y) continue;

                    if (row == -1) row = height - 1;
                    if (column == -1) column = width - 1;
                    if (row == height) row = 0;
                    if (column == width) column = 0;

                    if (grid[row, column]) NumOfAliveNeighbors++;
                }
            }
            return NumOfAliveNeighbors;
        }

        public bool CellNextState(bool[,] grid, int x, int y) //task 4
        {
            int numOfAliveNeighbors = GetNeighbors(grid, x, y);
            bool curCell = false;
            if (grid[x, y])
            {
                curCell = true;
                if (numOfAliveNeighbors < 2) curCell = false;
                if (numOfAliveNeighbors > 3) curCell = false;
            }
            else
            {
                if (numOfAliveNeighbors == 3) curCell = true;
            }
            return curCell;
        }

        public bool[,] NextGen(bool[,] grid) //task 5
        {
            bool[,] newgrid = new bool[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                newgrid[i, j] = CellNextState(grid, i, j);
                }
            }
            return newgrid;
        }

        public bool[,] GenResult(bool[,] grid, int gens) //task 6
        {
            int genCount = 0;
            while (genCount++ < gens)
            {
                grid = NextGen(grid);
            }
            return grid;
        }

        public void DrawGrid(int type /*0 == start ; any other int == result*/)
        {
            bool[,] grid = (type == 0) ? startGrid : resultGrid;
            //Console.SetCursorPosition(0, Console.WindowTop);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(grid[i, j] ? "x" : ".");
                }
                Console.WriteLine("\r");
            }
        }

        public string GridToString()
        {
            string matrix = "";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix += (resultGrid[i, j]) ? "x" : ".";

                }
                matrix += "\n";
            }
            return matrix;
        }
    }
}
