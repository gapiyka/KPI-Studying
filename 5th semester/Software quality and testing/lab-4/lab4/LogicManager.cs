namespace lab4
{
    public class LogicManager
    {
        public char[,] grid;
        public Shape shape;
        public Level level;
        public char[,] ExecRound(char[,] grid)
        {
            this.grid = grid;
            shape = new Shape(grid);
            level = new Level(grid);
            while (Update()) ;
            return this.grid;
        }

        public bool Update()
        {
            //check at possible move
            for (int row = shape.TopBorder; row <= shape.BottomBorder; row++)
            {
                for (int col = shape.LeftBorder; col <= shape.RightBorder; col++)
                {
                    if (grid[row, col] == 'p')
                    {
                        if (row == grid.GetLength(0) - 1) return false;
                        if (level.Body[row + 1, col] == '#') return false;
                    }
                }
            }
            shape.TopBorder++;
            shape.BottomBorder++;
            //fill the playground
            grid = (char[,])level.Body.Clone();
            for (int row = shape.TopBorder; row <= shape.BottomBorder; row++)
            {
                for (int col = shape.LeftBorder; col <= shape.RightBorder; col++)
                {
                    if (shape.Body[row - shape.TopBorder, col - shape.LeftBorder])
                        grid[row, col] = 'p';
                }
            }
            return true;
        }
    }
}
