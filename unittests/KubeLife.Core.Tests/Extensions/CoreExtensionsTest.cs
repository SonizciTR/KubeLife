using KubeLife.Core.Extensions;

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
    }
}