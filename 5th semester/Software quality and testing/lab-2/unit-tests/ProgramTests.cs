using System;
using Xunit;
using lab2;

namespace unit_tests
{
    public class ProgramTests
    {
        #region Tests
        [Theory]
        [InlineData("", new string[] { "" })]
        [InlineData("5", new string[] { "5" })]
        [InlineData("1 2", new string[] { "1", "2" })]
        [InlineData("1 2 3 + 4 5 6 =", new string[] { "1", "2", "3", "+", "4", "5", "6", "=" })]
        [InlineData("1 2 3 - 2 3 =", new string[] { "1", "2", "3", "-", "2", "3", "=" })]
        [InlineData("1 0 - 1 0 0 =", new string[] { "1", "0", "-", "1", "0", "0", "=" })]
        [InlineData("1 0 * 2 2 =", new string[] { "1", "0", "*", "2", "2", "=" })]
        [InlineData("1 0 0 / 3 =", new string[] { "1", "0", "0", "/", "3", "=" })]
        [InlineData("9 / 1 0 =", new string[] { "9", "/", "1", "0", "=" })]
        [InlineData("1 2 3 +", new string[] { "1", "2", "3", "+" })]
        [InlineData("1 2 3 + 4", new string[] { "1", "2", "3", "+", "4" })]
        [InlineData("1 2 3 + 4 5 6", new string[] { "1", "2", "3", "+", "4", "5", "6" })]
        [InlineData("- 5 =", new string[] { "-", "5", "=" })]
        [InlineData("abcd", new string[] { "abcd" })]
        public void ParseTest(string str, string[] arr)
        {
            //ACT
            string[] keys = Program.Parse(str);

            //ASSERT
            Assert.Equal(arr, keys);
        }

        [Theory]
        [InlineData(0, new string[] { "" })]
        [InlineData(5, new string[] { "5" })]
        [InlineData(12, new string[] { "1", "2" })]
        [InlineData(579, new string[] { "1", "2", "3", "+", "4", "5", "6", "=" })]
        [InlineData(100, new string[] { "1", "2", "3", "-", "2", "3", "=" })]
        [InlineData(-90, new string[] { "1", "0", "-", "1", "0", "0", "=" })]
        [InlineData(220, new string[] { "1", "0", "*", "2", "2", "=" })]
        [InlineData(33, new string[] { "1", "0", "0", "/", "3", "=" })]
        [InlineData(0, new string[] { "9", "/", "1", "0", "=" })]
        [InlineData(123, new string[] { "1", "2", "3", "+" })]
        [InlineData(4, new string[] { "1", "2", "3", "+", "4" })]
        [InlineData(456, new string[] { "1", "2", "3", "+", "4", "5", "6" })]
        [InlineData(-5, new string[] { "-", "5", "=" })]
        [InlineData(0, new string[] { "abcd" })]
        public void CalculateTest(int screen, string[] arr)
        {
            //ACT
            int res = Program.Calculate(arr);

            //ASSERT
            Assert.Equal(screen, res);
        }

        [Theory]
        [InlineData(false, 'a')]
        [InlineData(false, '2')]
        [InlineData(false, '\0')]
        [InlineData(true, '+')]
        [InlineData(true, '-')]
        [InlineData(true, '*')]
        [InlineData(true, '/')]
        [InlineData(false, '=')]
        public void IsOpTest(bool expected, char key)
        {
            //ACT
            bool res = Program.IsOp(key);

            //ASSERT
            Assert.Equal(expected, res);
        }

        [Theory]
        [InlineData(false, 'a')]
        [InlineData(false, '2')]
        [InlineData(false, '\0')]
        [InlineData(false, '+')]
        [InlineData(true, '=')]
        public void IsResTest(bool expected, char key)
        {
            //ACT
            bool res = Program.IsRes(key);

            //ASSERT
            Assert.Equal(expected, res);
        }

        [Theory]
        [InlineData(false, "a")]
        [InlineData(false, "")]
        [InlineData(false, "+")]
        [InlineData(false, "=")]
        [InlineData(true, "0")]
        [InlineData(true, "9")]
        [InlineData(true, "1")]
        [InlineData(true, "22")]
        public void IsDigitTest(bool expected, string key)
        {
            int n = 0;

            //ACT
            bool res = Program.IsDigit(key, ref n);

            //ASSERT
            Assert.Equal(expected, res);
        }

        [Fact]
        public void HandleKeyPressTest1()
        {
            //ARRANGE
            const int expectedScreen1 = 5;
            const int expectedScreen2 = 55;
            const int expectedFirstNum = 0;
            const char expectedOpChar = '\0';
            const bool expectedNewOperand = false;
            const string key1 = "5";
            const string key2 = "5";

            //ACT
            CalculatorState calculator = new CalculatorState();
            Program.HandleKeyPress(calculator, key1);
            int actualScreen1 = calculator.screen;
            int actualFirstnum1 = calculator.first_number;
            char actualOpChar1 = calculator.op;
            bool actualNewOperand1 = calculator.start_new_number;
            Program.HandleKeyPress(calculator, key2);
            int actualScreen2 = calculator.screen;
            int actualFirstnum2 = calculator.first_number;
            char actualOpChar2 = calculator.op;
            bool actualNewOperand2 = calculator.start_new_number;

            //ASSERT
            Assert.Equal(expectedScreen1, actualScreen1);
            Assert.Equal(expectedFirstNum, actualFirstnum1);
            Assert.Equal(expectedOpChar, actualOpChar1);
            Assert.Equal(expectedNewOperand, actualNewOperand1);
            Assert.Equal(expectedScreen2, actualScreen2);
            Assert.Equal(expectedFirstNum, actualFirstnum2);
            Assert.Equal(expectedOpChar, actualOpChar2);
            Assert.Equal(expectedNewOperand, actualNewOperand2);
        }


        [Fact]
        public void HandleKeyPressTest2()
        {
            //ARRANGE
            const int expectedScreen1 = 5;
            const int expectedScreen2 = 2;
            const int expectedFirstNum1 = 0;
            const int expectedFirstNum2 = expectedScreen1;
            const char expectedOpChar1 = '\0';
            const char expectedOpChar2 = '/';
            const bool expectedNewOperand1 = false;
            const bool expectedNewOperand2 = true;
            const string key1 = "5";
            const string key2 = "/";
            const string key3 = "2";
            const string key4 = "=";

            //ACT
            CalculatorState calculator = new CalculatorState();
            Program.HandleKeyPress(calculator, key1);
            int actualScreen1 = calculator.screen;
            int actualFirstnum1 = calculator.first_number;
            char actualOpChar1 = calculator.op;
            bool actualNewOperand1 = calculator.start_new_number;
            Program.HandleKeyPress(calculator, key2);
            int actualScreen2 = calculator.screen;
            int actualFirstnum2 = calculator.first_number;
            char actualOpChar2 = calculator.op;
            bool actualNewOperand2 = calculator.start_new_number;
            Program.HandleKeyPress(calculator, key3);
            int actualScreen3 = calculator.screen;
            int actualFirstnum3 = calculator.first_number;
            char actualOpChar3 = calculator.op;
            bool actualNewOperand3 = calculator.start_new_number;
            Program.HandleKeyPress(calculator, key4);
            int actualScreen4 = calculator.screen;
            int actualFirstnum4 = calculator.first_number;
            char actualOpChar4 = calculator.op;
            bool actualNewOperand4 = calculator.start_new_number;

            //ASSERT
            Assert.Equal(expectedScreen1, actualScreen1);
            Assert.Equal(expectedFirstNum1, actualFirstnum1);
            Assert.Equal(expectedOpChar1, actualOpChar1);
            Assert.Equal(expectedNewOperand1, actualNewOperand1);
            Assert.Equal(expectedScreen1, actualScreen2);
            Assert.Equal(expectedFirstNum2, actualFirstnum2);
            Assert.Equal(expectedOpChar2, actualOpChar2);
            Assert.Equal(expectedNewOperand2, actualNewOperand2);
            Assert.Equal(expectedScreen2, actualScreen3);
            Assert.Equal(expectedFirstNum2, actualFirstnum3);
            Assert.Equal(expectedOpChar2, actualOpChar3);
            Assert.Equal(expectedNewOperand1, actualNewOperand3);
            Assert.Equal(expectedScreen2, actualScreen4);
            Assert.Equal(expectedFirstNum2, actualFirstnum4);
            Assert.Equal(expectedOpChar2, actualOpChar4);
            Assert.Equal(expectedNewOperand1, actualNewOperand4);
        }

        #endregion
    }
}