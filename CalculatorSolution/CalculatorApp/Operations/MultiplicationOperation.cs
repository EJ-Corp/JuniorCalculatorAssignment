using System.Linq;

namespace CalculatorApp
{
    public class MultiplicationOperation : OperationBase
    {
        public override double Calculate()
        {
            double result = 1;

            foreach (var v in Values)
            {
                result *= v;
            }

            foreach (var op in SubOperations)
            {
                result *= op.Calculate();
            }

            return result;
        }
    }
}