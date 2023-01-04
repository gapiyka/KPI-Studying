using System;
using System.IO;

namespace lab2
{
    public class Program
    {
        const char op_plus = '+';
        const char op_minus = '-';
        const char op_div = '/';
        const char op_mltpl = '*';
        const char op_equal = '=';
        const string default_file = "..\\..\\..\\..\\input.txt";
        static void Main(string[] args)
        {
            string file = (args.Length > 0) ? args[0] : default_file;
            string keysStr = File.ReadAllLines(file)[0];
            string[] keys = Parse(keysStr);
            int result = Calculate(keys);
            File.WriteAllText("output.txt", result.ToString());
        }

        public static string[] Parse(string str)
        {
           return str.Split(" ");
        }

        public static bool IsDigit(string str, ref int num)
        {
            return Int32.TryParse(str, out num);
        }

        public static bool IsOp(char с)
        {
            return (с == op_plus || с == op_minus ||
                    с == op_div || с == op_mltpl) ? 
                    true : false;
        }

        public static bool IsRes(char c)
        {
            return (c == op_equal) ? true : false;
        }

        public static void HandleKeyPress(CalculatorState state, string key)
        {
            int num = 0;
            char c = (key != "") ? key[0] : default;
            if (IsDigit(key, ref num))
            {
                state.screen = (state.start_new_number) ? num : int.Parse(state.screen.ToString() + num.ToString()[0]);
                state.start_new_number = false;
            }
            else if (IsOp(c))
            {
                state.op = c;
                state.start_new_number = true;
                state.first_number = state.screen;
            }
            else if (IsRes(c))
            {
                switch (state.op)
                {
                    case op_plus:
                        state.screen = state.first_number + state.screen;
                        break;
                    case op_minus:
                        state.screen = state.first_number - state.screen;
                        break;
                    case op_mltpl:
                        state.screen = state.first_number * state.screen;
                        break;
                    case op_div:
                        state.screen = state.first_number / state.screen;
                        break;
                    default: break;
                } 
            }
        }

        public static int Calculate(string[] keys)
        {
            CalculatorState calculator = new CalculatorState();
            foreach (string key in keys)
            {
                HandleKeyPress(calculator, key);
            }
            Console.WriteLine("Calculator result = " + calculator.screen);
            return calculator.screen;
        }
    }
}
