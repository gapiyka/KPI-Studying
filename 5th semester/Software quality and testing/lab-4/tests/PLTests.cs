using lab4;
using Xunit;

namespace tests
{
    public class PLTests
    {
        [Fact]
        public void ExecRoundTest1()
        {
            //ARRAGNE
            char[,] res;
            LogicManager manager = new LogicManager();
            //ACT
            res = manager.ExecRound(Examples.C_INPUT_1);
            //ASSERT
            Assert.Equal(Examples.C_OUTPUT_1, res);
        }
        [Fact]
        public void ExecRoundTest2()
        {
            //ARRAGNE
            char[,] res;
            LogicManager manager = new LogicManager();
            //ACT
            res = manager.ExecRound(Examples.C_INPUT_2);
            //ASSERT
            Assert.Equal(Examples.C_OUTPUT_2, res);
        }
        [Fact]
        public void ExecRoundTest3()
        {
            //ARRAGNE
            char[,] res;
            LogicManager manager = new LogicManager();
            //ACT
            res = manager.ExecRound(Examples.C_INPUT_3);
            //ASSERT
            Assert.Equal(Examples.C_OUTPUT_3, res);
        }
        [Fact]
        public void ExecRoundTest4()
        {
            //ARRAGNE
            char[,] res;
            LogicManager manager = new LogicManager();
            //ACT
            res = manager.ExecRound(Examples.C_INPUT_4);
            //ASSERT
            Assert.Equal(Examples.C_OUTPUT_4, res);
        }
        [Fact]
        public void ExecRoundTest5()
        {
            //ARRAGNE
            char[,] res;
            LogicManager manager = new LogicManager();
            //ACT
            res = manager.ExecRound(Examples.C_INPUT_5);
            //ASSERT
            Assert.Equal(Examples.C_OUTPUT_5, res);
        }

        [Fact]
        public void UpdateTest1()
        {
            //ARRAGNE
            bool res;
            LogicManager manager = new LogicManager();
            manager.grid = Examples.C_INPUT_4;
            manager.shape = new Shape(Examples.C_INPUT_4);
            manager.level = new Level(Examples.C_INPUT_4);

            //ACT
            res = manager.Update();
            //ASSERT
            Assert.True(res);
        }
        [Fact]
        public void UpdateTest2()
        {
            //ARRAGNE
            bool res, res2, res3;
            LogicManager manager = new LogicManager();
            manager.grid = Examples.C_INPUT_3;
            manager.shape = new Shape(Examples.C_INPUT_3);
            manager.level = new Level(Examples.C_INPUT_3);

            //ACT
            res = manager.Update();
            res2 = manager.Update();
            res3 = manager.Update();
            //ASSERT
            Assert.True(res);
            Assert.True(res2);
            Assert.False(res3);
        }
    }
}
