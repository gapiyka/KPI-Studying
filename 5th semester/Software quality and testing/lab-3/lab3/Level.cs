namespace lab3
{
    public class Level
    {

        public char[,] Body { get; private set; }
        public int Width => this.Body.GetLength(1);
        public int Height => this.Body.GetLength(0);

        public Level(char[,] grid)
        {
            Body = new char[grid.GetLength(0), grid.GetLength(1)];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Body[y, x] = (grid[y, x] == '#') ? '#' : '.';
                }
            }
        }
    }
}
