using KubeLife.Core.Extensions;

namespace KubeLife.Core.Tests.Extensions
{
    public class CoreExtensionsTest
    {
        [Fact]
        public void IsAny_NullCheck_ReturnsFalse()
        {
            List<string> target = null;

            var rslt = target.IsAny();

            Assert.False(rslt);
        }

        [Fact]
        public void IsAny_EmptyCheck_ReturnsFalse()
        {
            List<string> target = new List<string>();

            var rslt = target.IsAny();

            Assert.False(rslt);
        }

        [Fact]
        public void IsAny_NonEmptyCheck_ReturnsTrue()
        {
            List<string> target = new List<string>();
            target.Add("a");

            var rslt = target.IsAny();

            Assert.True(rslt);
        }
    }
}