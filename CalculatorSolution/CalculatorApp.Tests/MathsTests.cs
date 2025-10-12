using Xunit;
using CalculatorApp;
using System.Collections.Generic;

namespace CalculatorApp.Tests
{
    public class MathsTests
    {
        //Simple operation Tests
        #region 
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

        [Fact]
        public void Substraction_ReturnsCorrectResult()
        {
            Operation operation = new Operation()
            {
                ID = Operator.Substraction,
                Value = new List<double>() { 20, 10 }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(operation);

            Assert.Equal(10, result);
        }

        [Fact]
        public void Division_ReturnsCorrectResult()
        {
            Operation operation = new Operation()
            {
                ID = Operator.Division,
                Value = new List<double>() { 13, 2 }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(operation);

            Assert.Equal(6.5, result);
        }
        #endregion

        #region
        [Fact]
        public void NestedOperation_ShouldReturnCorrectResult1()
        {
            //Test 2 + 3 + (4 * 5)
            Operation multiplication = new Operation()
            {
                ID = Operator.Multiplication,
                Value = new List<double>() { 4, 5 }
            };

            Operation addition = new Operation()
            {
                ID = Operator.Addition,
                Value = new List<double>() { 2, 3 },
                SubOperations = new List<Operation>() { multiplication }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(addition);

            Assert.Equal(25, result);
        }
        
        public void NestedOperation_ShouldReturnCorrectResult2()
        {
            //Test 10 - 7 + 5 * 3
            Operation multiplication = new Operation()
            {
                ID = Operator.Multiplication,
                Value = new List<double>() { 5, 3 }
            };

            Operation substraction = new Operation()
            {
                ID = Operator.Substraction,
                Value = new List<double>() { 10, 7 }
            };

            Operation addition = new Operation()
            {
                ID = Operator.Addition,
                SubOperations = new List<Operation>() { multiplication, substraction }
            };

            Maths maths = new Maths();
            double result = maths.Calculate(addition);

            Assert.Equal(18, result);
        }
        #endregion


        
    }
}