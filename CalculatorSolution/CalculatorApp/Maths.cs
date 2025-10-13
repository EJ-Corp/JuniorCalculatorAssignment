using System;

namespace CalculatorApp
{
    public class Maths
    {
        public double Calculate(Operation operation)
        {
            //conver the operation into a typed OperationBase
            OperationBase typedOp = OperationMaker.Create(operation);
            
            return typedOp.Calculate();
        }
    }
}