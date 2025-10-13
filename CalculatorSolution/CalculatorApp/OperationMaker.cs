using System;

namespace CalculatorApp
{
    public static class OperationMaker
    {
        public static OperationBase Create(Operation op)
        {
            OperationBase operationBase;

            switch (op.ID)
            {
                case Operator.Addition:
                    operationBase = new AdditionOperation();
                    break;

                case Operator.Substraction:
                    operationBase = new SubstractionOperation();
                    break;

                case Operator.Multiplication:
                    operationBase = new MultiplicationOperation();
                    break;

                case Operator.Division:
                    operationBase = new DivisionOperation();
                    break;

                default:
                    throw new InvalidOperationException($"Unknown operator: {op.ID}");
            }

            operationBase.Values.AddRange(op.Value);

            foreach (var subOp in op.SubOperations)
            {
                operationBase.SubOperations.Add(Create(subOp));
            }

            return operationBase;
        }
    }
}