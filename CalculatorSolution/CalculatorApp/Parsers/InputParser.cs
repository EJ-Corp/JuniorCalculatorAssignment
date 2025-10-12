using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CalculatorApp;

namespace CalculatorApp.Parsers
{
    public static class InputPaser
    {
        //Map the ID in string version to an Operator constant 
        public static Operator ParseOperatorID(string idOrName)
        {
            //Check for null or empty string
            if (string.IsNullOrWhiteSpace(idOrName))
            {
                throw new ArgumentException("Operator id/name is missing");
            }

            //Remove white space and make everything lowecase
            var s = idOrName.Trim().ToLowerInvariant();

            //Check for different ways to say plus 
            if (s.Contains("plus") || s.Contains("add") || s.Contains("addition"))
            {
                return Operator.Addition;
            }

            //Check for different ways to say minus 
            if (s.Contains("minus") || s.Contains("subtract") || s.Contains("substraction"))
            {
                return Operator.Substraction;
            }

            //Check for different ways to say multiply 
            if (s.Contains("multiply") || s.Contains("times") || s.Contains("multiplication"))
            {
                return Operator.Multiplication;
            }

            //Check for different ways to say divide 
            if (s.Contains("div") || s.Contains("division") || s.Contains("/") || s.Contains("divide"))
            {
                return Operator.Division;
            }

            //Any other = error
            throw new InvalidOperationException($"Unkown operator id: {idOrName}");
        }

        //Check if elemnt is an operation by checking for an ID
        private static bool IsOperation(XElement el)
        {
            //null check
            if (el == null)
            {
                return false;
            }

            //Check for different spellings of ID
            if (el.Attribute("ID") != null || el.Attribute("Id") != null || el.Attribute("id") != null)
            {
                return true;
            }

            var name = el.Name.LocalName.ToLowerInvariant();
            return name.Contains("operation");
        }
    }
}