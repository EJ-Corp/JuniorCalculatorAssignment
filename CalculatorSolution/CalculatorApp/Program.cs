using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Operation addOperation = new Operation()
            {
                ID = Operator.Addition,
                Value = new List<double>() { 3, 4 }
            };

            Maths maths = new Maths();

            double result = maths.Calculate(addOperation);
        }

    }
}