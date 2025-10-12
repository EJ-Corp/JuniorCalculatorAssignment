using Xunit;
using CalculatorApp;
using System.Collections.Generic;

namespace CalculatorApp.Tests
{
    public class MathsTests
    {
        [Fact]
        public void Addition_ShouldReturnSum()
        {
            Operation operation = new Operation()
            {
                ID = Operator.Addition,
                Value = new List<double>() { 2, 3, 5 }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(operation);

            Assert.Equal(10, result);
        }

        [Fact]
        public void Multiplication_ShouldReturnProduct()
        {
            Operation operation = new Operation()
            {
                ID = Operator.Multiplication,
                Value = new List<double>() { 4, 5 }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(operation);

            Assert.Equal(20, result);
        }
    }
}