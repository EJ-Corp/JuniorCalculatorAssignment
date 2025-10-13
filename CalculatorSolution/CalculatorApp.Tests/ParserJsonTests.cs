using Xunit;
using CalculatorApp.Parsers;

namespace CalculatorApp.Tests
{
    public class ParserJsonTests
    {
        [Fact]
        public void Parse_SimpleJson_ReturnsOperationTree()
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
    }
}