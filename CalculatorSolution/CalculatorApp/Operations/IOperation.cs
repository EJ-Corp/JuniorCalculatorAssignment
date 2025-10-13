using System.Collections.Generic;

namespace CalculatorApp
{
    public interface IOperation
    {
        double Calculate();
        List<double> Values { get; }
        List<IOperation> SubOperations { get; }
    }
}