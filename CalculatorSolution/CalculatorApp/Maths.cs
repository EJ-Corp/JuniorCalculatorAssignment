using System;

namespace CalculatorApp
{
    public class Maths
    {
        public double Calculate(Operation operation)
        {
            double result = 0;

            //Do all nested operations first
            foreach(var subOp in operation.SubOperations)
            {
                double subResult = Calculate(subOp);
                operation.Value.Add(subResult);
            }

            switch(operation.ID)
            {
                //Addition Behaviour
                case Operator.Addition:
                    foreach (var v in operation.Value)
                    {
                        result += v;
                    }
                    break;
                
                //Substraction Behaviour
                case Operator.Substraction:
                    result = operation.Value[0];
                    for(int i = 1; i < operation.Value.Count; i++)
                    {
                        result -= operation.Value[i];
                    }
                    break;

                //Multiplication Behaviour
                case Operator.Multiplication:
                    result = 1;
                    foreach(var v in operation.Value)
                    {
                        result *= v;
                    }
                    break;

                //Division Behaviour
                case Operator.Division:
                    result = operation.Value[0];
                    for(int i = 1; i < operation.Value.Count; i++)
                    {
                        result /= operation.Value[i];
                    }
                    break;

                //Throw error for anything not matching the outlined constants
                default:
                    throw new InvalidOperationException("Unkown Operator");
            }

            return result;
        }
    }
}