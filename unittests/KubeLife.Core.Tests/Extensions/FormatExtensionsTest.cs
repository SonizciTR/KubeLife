using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KubeLife.Core.Extensions;

namespace KubeLife.Core.Tests.Extensions
{
    public class FormatExtensionsTest
    {
        [Fact]
        public void ToDateFormat_FormatControl_ShouldReturnPreDetirmened()
        {
            var timeConst = new DateTime(2023, 2, 1, 14, 3, 4);
            var expected = "01.02.2023 14:03:04";

            var rslt = timeConst.ToDateFormat();

            Assert.Equal(expected, rslt);
        }
    }
}
