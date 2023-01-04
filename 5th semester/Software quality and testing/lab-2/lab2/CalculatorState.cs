using System;

namespace lab2
{
    public class CalculatorState
    {
        public int screen;
        public int first_number;
        public char op;
        public bool start_new_number;

        public CalculatorState()
        {
            this.screen = default;
            this.first_number = default;
            this.op = default;
            this.start_new_number = true;
        }
    }
}
