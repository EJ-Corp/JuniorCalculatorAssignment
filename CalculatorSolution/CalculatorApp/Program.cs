using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //Try sample equation: 2 + 3 + (4 * 5)
            Operation multiplyOperation = new Operation()
            {
                ID = Operator.Multiplication,
                Value = new List<double>() { 4, 5 }
            };
            
            Operation addOperation = new Operation()
            {
                ID = Operator.Addition,
                Value = new List<double>() { 2, 3 },
                SubOperations = new List<Operation>() {multiplyOperation}
            };

            Maths maths = new Maths();
            double result = maths.Calculate(addOperation);

            Console.WriteLine($"Result: {result}");
        }

    }
}