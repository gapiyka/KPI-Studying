namespace lab4
{
    public class Shape
    {

        public bool[,] Body { get; private set; }
        public int Width => this.Body.GetLength(1);
        public int Height => this.Body.GetLength(0);
        public int LeftBorder;
        public int RightBorder;
        public int TopBorder;
        public int BottomBorder;

        public Shape(char[,] grid)
        {
            int gridWidth = grid.GetLength(1);
            int gridHeight = grid.GetLength(0);
            LeftBorder = gridWidth;
            RightBorder = 0;
            TopBorder = gridHeight;
            BottomBorder = 0;
            int counterX = 0, counterY = 0;
            // find submatrix with "live" pixels
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    if (grid[y, x] == 'p')
                    {
                        if (x < LeftBorder) LeftBorder = x;
                        if (x > RightBorder) RightBorder = x;
                        if (y < TopBorder) TopBorder = y;
                        if (y > BottomBorder) BottomBorder = y;
                    }
                }
            }
            Body = new bool[BottomBorder - TopBorder + 1, RightBorder - LeftBorder + 1];
            // cut a submatrix
            for (int y = TopBorder; y <= BottomBorder; y++)
            {
                for (int x = LeftBorder; x <= RightBorder; x++)
                {
                    Body[counterY, counterX] = (grid[y, x] == 'p') ? true : false;
                    counterX++;
                }
                counterY++;
                counterX = 0;
            }
        }
    }
}
