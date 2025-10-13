using System.Collections.Generic;

namespace CalculatorApp
{
    public abstract class OperationBase : IOperation
    {
        public List<double> Values { get; set; } = new List<double>();
        public List<IOperation> SubOperations { get; set; } = new List<IOperation>();

        public abstract double Calculate();
    }
}