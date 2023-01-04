using System;
using Xunit;
using lab2;

namespace unit_tests
{
    public class CalculatorStateTests
    {
        [Fact]
        public void ConstructorTest()
        {
            //ARRANGE
            const int expectedScreen = 0;
            const int expectedFirstNum = 0;
            const char expectedOpChar = '\0';
            const bool expectedNewOperand = true;

            //ACT
            CalculatorState calculator = new CalculatorState();

            //ASSERT
            Assert.Equal(expectedScreen, calculator.screen);
            Assert.Equal(expectedFirstNum, calculator.first_number);
            Assert.Equal(expectedOpChar, calculator.op);
            Assert.Equal(expectedNewOperand, calculator.start_new_number);
        }
    }
}