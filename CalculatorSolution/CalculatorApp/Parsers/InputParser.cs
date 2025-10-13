using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CalculatorApp;
using Newtonsoft.Json.Linq;

namespace CalculatorApp.Parsers
{
    public static class InputPaser
    {
        #region Shared Helper Methods

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

        #endregion

        #region XML Specific Methods

        //Convert XElement to an operation object 
        private static Operation PareseOperationElement(XElement el)
        {
            var operation = new Operation
            {
                Value = new List<double>(),
                SubOperations = new List<Operation>()
            };

            //Get the opertator ID with checking for different spelling
            var idAttribute = el.Attribute("ID")?.Value ?? el.Attribute("Id")?.Value ?? el.Attribute("id")?.Value;
            operation.ID = ParseOperatorID(idAttribute ?? el.Name.LocalName);

            //Now parse the value 
            foreach (var v in el.Elements().Where(x => x.Name.LocalName.Equals("Value", StringComparison.OrdinalIgnoreCase)))
            {
                if (double.TryParse(v.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                {
                    operation.Value.Add(d);
                }
                else
                {
                    throw new FormatException("Value: " + v.Value + " is not a valid number");
                }
            }

            //Parsing nested operations
            foreach (var child in el.Elements().Where(IsOperation))
            {
                operation.SubOperations.Add(PareseOperationElement(child));
            }

            return operation;
        }

        //Find an element that is an operation
        private static XElement FindOperationElement(XElement node)
        {
            //Found an operation so send it up
            if (IsOperation(node))
            {
                return node;
            }

            //Check for nested operations
            foreach (var child in node.Elements())
            {
                var found = FindOperationElement(child);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
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

        //Parse an XML file into an Operation Object
        public static Operation ParseXml(string xml)
        {
            //Check for empty file
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentException("input is empty");
            }

            var doc = XDocument.Parse(xml);
            var root = doc.Root ?? throw new ArgumentException("XML root not found");

            //Find the first operation element
            var opElement = FindOperationElement(root);
            if (opElement == null)
            {
                throw new ArgumentException("No operation element found in input");
            }

            //Conver the XElement to a Operation and return
            return PareseOperationElement(opElement);
        }

        #endregion

        #region JSON Specific Methods

        //COnvert JProperty into Operation Object
        private static Operation ParseOperationJson(JProperty prop)
        {
            if (!(prop.Value is JObject obj))
            {
                throw new ArgumentException($"Expected JObject for operation '{prop.Name}'");
            }
            
            Operation operation = new Operation()
            {
                Value = new List<double>(),
                SubOperations = new List<Operation>()
            };

            //Find the ID of the operator
            string id = obj["@ID"]?.ToString() ?? prop.Name?? throw new ArgumentException("Cannot determine operator ID");
            operation.ID = ParseOperatorID(id);

            //Parse the values for operation
            var valuesToken = obj["Value"];
            if (valuesToken != null)
            {
                foreach (var v in valuesToken)
                {
                    if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                    {
                        operation.Value.Add(d);
                    }
                    else
                    {
                        throw new FormatException("Value " + v + " is not a valid number");
                    }
                }
            }

            //Parse nested operations into operation objects
            foreach (var childProp in obj.Properties())
            {
                if (childProp.Name != "Value" && childProp.Name != "@ID")
                {
                    operation.SubOperations.Add(ParseOperationJson(childProp));
                }
            }

            return operation;
        }

        //Parse a JSON file into an Operarion Object
         public static Operation ParseJson(string json)
        {
            var jObject = JObject.Parse(json);

            var rootProp = jObject.Properties().First();
            var firstOperationProp = rootProp.Value.Children<JProperty>().FirstOrDefault();

            if (firstOperationProp == null)
            {
                throw new ArgumentException("No operation found in JSON");
            }
                
            return ParseOperationJson(firstOperationProp);
        }
        
        #endregion
        
    }
}