using Xunit;
using CalculatorApp.Parsers;

namespace CalculatorApp.Tests
{
    public class ParserJsonTests
    {
        [Fact]
        public void Parse_SampleJson_ReturnsOperationTree()
        {
            var json = @"{
                'Maths': {
                    'Operation': {
                        '@ID': 'Plus',
                        'Value': ['2','3'],
                        'Operation': {
                            '@ID': 'Multiplication',
                            'Value': ['4','5']
                        }
                    }
                }
            }".Replace('\'', '"');

            var op = InputPaser.ParseJson(json);

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
        public void Parse_MultiOperationJson_CalculatesCorrectly()
        {
            var json = @"{
                'Maths': {
                    'Operation': {
                        '@ID': 'Plus',
                        'Value': ['1','2'],
                        'Operation1': {
                            '@ID': 'Multiplication',
                            'Value': ['3','4'],
                            'Operation2': {
                                '@ID': 'Substraction',
                                'Value': ['10','5']
                            }
                        }
                    }
                }
            }".Replace('\'', '"');

            var op = InputPaser.ParseJson(json);

            // Check top-level
            Assert.Equal(Operator.Addition, op.ID);
            Assert.Equal(2, op.Value.Count);
            Assert.Single(op.SubOperations);

            // Check first nested
            var mult = op.SubOperations[0];
            Assert.Equal(Operator.Multiplication, mult.ID);
            Assert.Equal(2, mult.Value.Count);
            Assert.Single(mult.SubOperations);

            // Check second nested
            var sub = mult.SubOperations[0];
            Assert.Equal(Operator.Substraction, sub.ID);
            Assert.Equal(2, sub.Value.Count);
        }

        [Fact]
        public void Parse_SampleJson_NonNumericValues()
        {
            var json = @"{
                'Maths': {
                    'Operation': {
                        '@ID': 'Plus',
                        'Value': ['2','abc']
                    }
                }
            }".Replace('\'', '"');

            Assert.Throws<FormatException>(() => InputPaser.ParseJson(json));
        }

        [Fact]
        public void Parse_SampleJson_UnkownOperator()
        {
            var json = @"{
                'Maths': {
                    'Operation': {
                        '@ID': 'Power',
                        'Value': ['2','3']
                    }
                }
            }".Replace('\'', '"');

            Assert.Throws<InvalidOperationException>(() => InputPaser.ParseJson(json));
        }

        [Fact]
        public void Parse_SampleJson_CalculatesCorrectly()
        {
            var json = @"{
                'Maths': {
                    'Operation': {
                        '@ID': 'Plus',
                        'Value': ['2','3'],
                        'Operation': {
                            '@ID': 'Multiplication',
                            'Value': ['4','5']
                        }
                    }
                }
            }".Replace('\'', '"');

            var op = InputPaser.ParseJson(json);

            Maths maths = new Maths();
            double result = maths.Calculate(op);

            Assert.Equal(25, result);
        }
    }
}