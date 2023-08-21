using KubeLife.Core.Extensions;
using KubeLife.Core.Tests.Extensions.Mocks;

namespace KubeLife.Core.Tests.Extensions
{
    public class CoreExtensionsTest
    {
        public static IEnumerable<object[]> dataForIsAny => new List<object[]> {
                            new object[]{ null, false },
                            new object[]{ new List<string>(), false },
                            new object[]{ new List<string> { "1"  }, true },
        };

        [Theory, MemberData(nameof(dataForIsAny))]
        public void IsAny_PossibleValuesCheck_DynamicResult(List<string> data, bool expectedResult)
        {
            var rslt = data.IsAny();

            Assert.Equal(expectedResult, rslt);
        }

        [Fact]
        public void CasttoList_ArrayBasedAssignValue_ShouldBeConvertedTargetClass()
        {
            var source = new SimpleClass1[] { new SimpleClass1("1"), new SimpleClass1("2") };

            SimpleClass2 CastInner(SimpleClass1 source)
            {
                var tmp = new SimpleClass2();
                tmp.Name2 = source.Name1;
                return tmp;
            }

            var target = CoreExtensions.CasttoList<SimpleClass2, SimpleClass1>(source, CastInner);

            Assert.Equal(source.Length, target.Count);
            Assert.Equal(source[0].Name1, target[0].Name2);
        }

        [Fact]
        public void CasttoList_ListBasedAssignValue_ShouldBeConvertedTargetClass()
        {
            var source = new List<SimpleClass1> { new SimpleClass1("1"), new SimpleClass1("2") };

            SimpleClass2 CastInner(SimpleClass1 source)
            {
                var tmp = new SimpleClass2();
                tmp.Name2 = source.Name1;
                return tmp;
            }

            var target = CoreExtensions.CasttoList<SimpleClass2, SimpleClass1>(source, CastInner);

            Assert.Equal(source.Count, target.Count);
            Assert.Equal(source[0].Name1, target[0].Name2);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        [InlineData(3, 3)]
        public void ChangeType_TypeConversion_CastingShouldBeOk(object rawValue, int expectedValue)
        {
            var target = rawValue.ChangeType<int>();
            Assert.True(target == expectedValue);
        }

        [Theory]
        [InlineData("12345677890")]
        [InlineData("!'^+%&/()=")]
        [InlineData("qwerrtyuop")]
        [InlineData("ğüşiöçı")]
        public void ToStringForm_StreamShouldBeConverted_StringShoulBeMatched(string expected)
        {
            var strmTarget = new MemoryStream();
            var writer = new StreamWriter(strmTarget);
            writer.Write(expected);
            writer.Flush();
            strmTarget.Position = 0;
            
            var actual = strmTarget.ToStringForm();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringJoin_ListToStringWithCustomChar()
        {
            var data = new List<string> { "a", "b", "c" };

            var target = data.StringJoin("*");

            Assert.Matches("a*b*c", target.ToString());
        }
    }
}