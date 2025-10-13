using System.Collections.Generic;

namespace CalculatorApp
{
    public class Operation
    {
        public List<double> Value { get; set; } = new List<Double>();
        public Operator ID { get; set; }

        //Store nested operations to set up for recursion
        public List<Operation> SubOperations { get; set; } = new List<Operation>();
    }
}