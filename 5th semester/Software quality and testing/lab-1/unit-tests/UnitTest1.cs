using System;
using Xunit;
using lab1;
using System.IO;

namespace unit_tests
{
    public class UnitTest1
    {
        [Fact]
        public void SizeTest1()
        {
            //ARRANGE
            int expectedHeight = 5;
            int expectedWidth = 5;

            //ACT
            CGoL game = new CGoL();
            int resultHeight = game.Height;
            int resultWidth = game.Width;

            //ASSERT
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedWidth, resultWidth);
        }

        [Fact]
        public void SizeTest2()
        {
            //ARRANGE
            int expectedHeight = 5;
            int expectedWidth = 8;

            //ACT
            CGoL game = new CGoL(expectedHeight, expectedWidth, 2, strS1);
            int resultHeight = game.Height;
            int resultWidth = game.Width;

            //ASSERT
            Assert.Equal(expectedHeight, resultHeight);
            Assert.Equal(expectedWidth, resultWidth);
        }

        [Fact]
        public void SizeTest3()
        {
            //ARRANGE
            int tryHeight = -1;
            int expectedWidth = 8;
            CGoL game;


            //ACT
            void Act()
            {
                game = new CGoL(tryHeight);
            }


            //ASSERT
            Assert.Throws(typeof(OverflowException), Act);
        }

        static readonly bool[,] startGrid1 = 
            {{false, false, false, false, false, false, false, false},
            {false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false},
            {false, false, true, false, false, false, false, false},
            {false, false, false, false, false, false, false, false}};
        static readonly bool[,] expectedGrid1 = 
            {{false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false},
            {false, true, true, true, false, false, false, false},
            {false, false, false, false, false, false, false, false},
            {false, false, false, false, false, false, false, false}};
        static readonly bool[,] startGrid2 = 
            {{true, true, false, false},
            {true, true, false, false},
            {false, false, false, false},
            {false, false, false, false}};
        static readonly bool[,] startGrid3 = 
            {{false, true, true, false},
            {true, false, true, false},
            {false, false, false, true},
            {false, false, false, false}};
        static readonly bool[,] expectedGrid3 = 
            {{true, false, false, true},
            {true, false, true, false},
            {false, false, true, true},
            {false, false, false, false}};
        static readonly string strS1 = "........\n..x.....\n..x.....\n..x.....\n........\n";
        static readonly string strS2 = "xx..\nxx..\n....\n....\n";
        static readonly string strS3 = ".xx.\nx.x.\n...x\n....\n";
        static readonly string strE3 = "x..x\nx.x.\n..xx\n....\n";

       [Fact]
        public void StringToMatrixTest1()
        {
            //ACT
            CGoL game = new CGoL(5, 8, 2, strS1);
            bool[,] actualGrid = game.StringToMatrix(strS1);

            //ASSERT
            Assert.Equal(startGrid1, actualGrid);
        }

        [Fact]
        public void StringToMatrixTest2()
        {
            //ACT
            CGoL game = new CGoL(4,4);
            bool[,] actualGrid = game.StringToMatrix(strS2);

            //ASSERT
            Assert.Equal(startGrid2, actualGrid);
        }
        
        [Fact]
        public void GetNeighborsTest1()
        {
            //ASSERT
            int expectedNeighborsCount = 3;
            int xPos = 0;
            int yPos = 0;

            //ACT
            CGoL game = new CGoL(4, 4);
            int actualNeighborsCount = game.GetNeighbors(startGrid2, xPos, yPos);

            //ASSERT
            Assert.Equal(expectedNeighborsCount, actualNeighborsCount);
        }
        
        [Fact]
        public void GetNeighborsTest2()
        {
            //ASSERT
            int expectedNeighborsCount = 1;
            int xPos = 2;
            int yPos = 2;

            //ACT
            CGoL game = new CGoL(4, 4);
            int actualNeighborsCount = game.GetNeighbors(startGrid2, xPos, yPos);

            //ASSERT
            Assert.Equal(expectedNeighborsCount, actualNeighborsCount);
        }
        
        [Fact]
        public void GetNeighborsTest3()
        {
            //ASSERT
            int expectedNeighborsCount = 1;
            int xPos = 3;
            int yPos = 3;

            //ACT
            CGoL game = new CGoL(4, 4);
            int actualNeighborsCount = game.GetNeighbors(startGrid2, xPos, yPos);

            //ASSERT
            Assert.Equal(expectedNeighborsCount, actualNeighborsCount);
        }

        [Fact]
        public void CellNextStateTest1()
        {
            //ASSERT
            bool expectedCellState = false;
            int xPos = 3;
            int yPos = 0;

            //ACT
            CGoL game = new CGoL(4, 4);
            bool actualCellState = game.CellNextState(startGrid2, xPos, yPos);

            //ASSERT
            Assert.Equal(expectedCellState, actualCellState);
        }

        [Fact]
        public void CellNextStateTest2()
        {
            //ASSERT
            bool expectedCellState = true;
            int xPos = 0;
            int yPos = 0;

            //ACT
            CGoL game = new CGoL(4, 4);
            bool actualCellState = game.CellNextState(startGrid2, xPos, yPos);

            //ASSERT
            Assert.Equal(expectedCellState, actualCellState);
        }

        [Fact]
        public void NextGenTest()
        {
            //ACT
            CGoL game = new CGoL(5, 8, 1, strS1);
            bool[,] newGrid = game.NextGen(startGrid1);

            //ASSERT
            Assert.Equal(expectedGrid1, newGrid);
        }
        
        [Fact]
        public void GenResultTest()
        {
            //ACT
            CGoL game = new CGoL(4, 4, 3, strS3);
            bool[,] newGrid = game.GenResult(startGrid3, 3);

            //ASSERT
            Assert.Equal(expectedGrid3, newGrid);
        }

        [Fact]
        public void DrawGridTest()
        {
            //ARRANGE
            var stringWriter = new StringWriter();

            //ACT
            Console.SetOut(stringWriter);
            CGoL game = new CGoL(4, 4, 3, strS3);
            game.DrawGrid(1);
            string res = stringWriter.ToString().Replace("\r", "");

            //ASSERT
            Assert.Equal(strE3, res);
        }

        [Fact]
        public void GridToStringTest()
        {
            //ACT
            CGoL game = new CGoL(4, 4, 0, strS2);
            string actualString = game.GridToString();

            //ASSERT
            Assert.Equal(strS2, actualString);
        }
    }
}
