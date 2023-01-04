using System;
using lab4;
using Xunit;

namespace tests
{
    public class MockIOManager : IOManager // mock or more similar to fake, idk, i skipped 8-th lec due to blackout ;)
    {
        public string existingPath = "input.txt";
        public string existingFileContent = Examples.INPUT_1;
        public string result = "";
        public override string ParseFile(string file)
        {
            if (file == "") file = AskFileName();
            return (file == existingPath) ? existingFileContent : Constants.ER_FILE_NOT_EXIST;
        }
        public override string AskFileName()
        {
            Console.WriteLine("Please, input file path / name: ");
            return existingPath;
        }
        public override void GiveAnswer(string text)
        {
            Console.WriteLine(text);
            result = text;
        }
    }
    public class CLTests
    {
        [Fact]
        public void ConstructorAndRunMockTest()
        {
            //ARRANGE
            MockIOManager mock = new MockIOManager();
            LogicManager logic = new LogicManager();
            string[] args = new string[] { "" };

            //ACT
            CommunicationLayer communication = new CommunicationLayer(mock, logic, args);

            //ASSERT
            Assert.NotNull(communication.ioManager);
            Assert.NotNull(communication.logicManager);
            Assert.Equal("", communication.inputFile);
            Assert.Equal(Examples.INPUT_1, communication.inputResult);
            Assert.Equal(mock.result, communication.outputResult);
            Assert.Equal(Examples.OUTPUT_1, mock.result);
        }

        [Fact]
        public void ExceptionsMockedTest1()
        {
            //ARRANGE
            MockIOManager mock = new MockIOManager();
            LogicManager logic = new LogicManager();
            string[] args = new string[] { "abc" };
            CommunicationLayer communication;

            //ACT
            Action act = () => communication = new CommunicationLayer(mock, logic, args);

            //ASSERT
            Exception x_x = Assert.Throws<Exception>(act);
            Assert.Equal("Sorry, but this file doesn't exist in directory.", x_x.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("5 2 7\n#.#")]
        [InlineData("0 2\n##\n##")]
        [InlineData("2 0\n##\n##")]
        [InlineData("2 2\n####\n##")]
        [InlineData("2 2\n##\n##\n##")]
        public void ExceptionsMockedTest2(string content)
        {
            //ARRANGE
            MockIOManager mock = new MockIOManager();
            LogicManager logic = new LogicManager();
            string[] args = new string[] { };
            CommunicationLayer communication;

            //ACT
            mock.existingFileContent = content;
            Action act = () => communication = new CommunicationLayer(mock, logic, args);

            //ASSERT
            Exception x_x = Assert.Throws<Exception>(act);
            Assert.Equal("Sorry, but grid parameters are wrong.", x_x.Message);
        }

        [Theory]
        [InlineData(Examples.INPUT_1, Examples.OUTPUT_1)]
        [InlineData(Examples.INPUT_2, Examples.OUTPUT_2)]
        [InlineData(Examples.INPUT_3, Examples.OUTPUT_3)]
        [InlineData(Examples.INPUT_4, Examples.OUTPUT_4)]
        [InlineData(Examples.INPUT_5, Examples.OUTPUT_5)]
        public void MockedTest(string content, string res)
        {
            //ARRANGE
            MockIOManager mock = new MockIOManager();
            LogicManager logic = new LogicManager();
            string[] args = new string[] { };
            CommunicationLayer communication;

            //ACT
            mock.existingFileContent = content;
            communication = new CommunicationLayer(mock, logic, args);


            //ASSERT
            Assert.Equal(content, communication.inputResult);
            Assert.Equal(res, mock.result);
        }
    }
}
