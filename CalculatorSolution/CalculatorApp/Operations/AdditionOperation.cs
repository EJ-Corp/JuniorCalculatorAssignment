using System.Linq;

namespace CalculatorApp
{
    public class AdditionOperation : OperationBase
    {
        public override double Calculate()
        {
            double result = Values.Sum();

            foreach (var op in SubOperations)
            {
                result += op.Calculate();
            }

            return result;
        }
    }
}