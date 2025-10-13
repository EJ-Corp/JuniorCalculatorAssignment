using System.Linq;
using System;

namespace CalculatorApp
{
    public class DivisionOperation : OperationBase
    {
        public override double Calculate()
        {
            //Check for empty Values
            if (!Values.Any())
            {
                throw new InvalidOperationException("Division operation requires at least one value.");
            }

            double result = Values.First();

            for (int i = 1; i < Values.Count; i++)
            {
                //Can't divide by 0
                if (Values[i] == 0)
                {
                    throw new DivideByZeroException();
                }

                result /= Values[i];
            }

            foreach (var op in SubOperations)
            {
                double subResult = op.Calculate();
                result /= subResult;
            }

            return result;
        }
    }
}