using System.Linq;

namespace CalculatorApp
{
    public class SubstractionOperation : OperationBase
    {
        public override double Calculate()
        {
            //Check for empty values
            if (!Values.Any() && !SubOperations.Any())
            {
                return 0;
            }

            double result = Values.FirstOrDefault();

            for (int i = 1; i < Values.Count; i++)
            {
                result -= Values[i];
            }

            foreach (var op in SubOperations)
            {
                result -= op.Calculate();
            }

            return result;
        }
    }
}