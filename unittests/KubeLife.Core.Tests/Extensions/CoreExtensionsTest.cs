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
    }
}