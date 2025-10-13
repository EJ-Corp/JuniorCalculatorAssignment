using Xunit;
using CalculatorApp.Parsers;

namespace CalculatorApp.Tests
{
    public class ParserXmlTests
    {
        [Fact]
        public void Parse_SampleXml_ReturnsOperationTree()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <Maths>
                                <Operation ID=""Plus"">
                                    <Value>2</Value>
                                    <Value>3</Value>
                                        <Operation ID=""Multiplication"">
                                            <Value>4</Value>
                                            <Value>5</Value>
                                        </Operation> 
                                </Operation>
                            </Maths>";

            var op = InputPaser.ParseXml(xml);

            Assert.Equal(Operator.Addition, op.ID);
            Assert.Equal(2, op.Value.Count);
            Assert.Equal(2, op.Value[0]);
            Assert.Equal(3, op.Value[1]);

            Assert.Single(op.SubOperations);
            var nested = op.SubOperations[0];
            Assert.Equal(Operator.Multiplication, nested.ID);
            Assert.Equal(2, nested.Value.Count);
            Assert.Equal(4, nested.Value[0]);
            Assert.Equal(5, nested.Value[1]);
        }

        [Fact]
        public void Parse_SampleXml_VariantNames_MyOperation_Works()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <MyMaths>
                                <MyOperation ID=""Plus"">
                                    <Value>2</Value>
                                    <Value>3</Value>
                                        <MyOperation ID=""Multiplication"">
                                            <Value>4</Value>
                                            <Value>5</Value>
                                        </MyOperation>
                                </MyOperation>
                            </MyMaths>";

            var op = InputPaser.ParseXml(xml);

            Assert.Equal(Operator.Addition, op.ID);
            Assert.Equal(2, op.Value.Count);
            Assert.Single(op.SubOperations);
            Assert.Equal(Operator.Multiplication, op.SubOperations[0].ID);
        }

        [Fact]
        public void Parse_SampleXml_CalculatesCorrectly()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <Maths>
                                <Operation id=""Plus"">
                                    <Value>2</Value>
                                    <Value>3</Value>
                                        <Operation ID=""Multiplication"">
                                            <Value>4</Value>
                                            <Value>5</Value>
                                        </Operation>
                                </Operation>
                            </Maths>";

            var op = InputPaser.ParseXml(xml);

            Maths maths = new Maths();
            double result = maths.Calculate(op);

            Assert.Equal(25, result);
        }

        [Fact]
        public void Parse_SampleXml_UnkownOperator()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <Maths>
                                <Operation id=""Happy"">
                                    <Value>2</Value>
                                    <Value>3</Value>
                                        <Operation ID=""Multiplication"">
                                            <Value>4</Value>
                                            <Value>5</Value>
                                        </Operation>
                                </Operation>
                            </Maths>";

            var except = Assert.Throws<InvalidOperationException>(() =>
            {
                var op = InputPaser.ParseXml(xml);

                Maths maths = new Maths();
                double result = maths.Calculate(op);
            });
        }

        [Fact]
        public void Parse_SampleXml_EmptyInput()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <Maths>
                                
                            </Maths>";

            var except = Assert.Throws<ArgumentException>(() =>
            {
                var op = InputPaser.ParseXml(xml);

                Maths maths = new Maths();
                double result = maths.Calculate(op);
            });
        }
    }
}