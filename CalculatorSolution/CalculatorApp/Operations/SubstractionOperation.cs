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

            double result;

            if(Values.Any())
            {
                result = Values.First();
                for (int i = 1; i < Values.Count; i++)
                {
                    result -= Values[i];
                }
            } else
            {
                result = SubOperations.First().Calculate();
                for (int i = 1; i < SubOperations.Count; i++)
                {
                    result -= SubOperations[i].Calculate();
                }

                return result;
            }

                foreach (var op in SubOperations)
                {
                    result -= op.Calculate();
                }

            return result;
        }
    }
}