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

        [Fact]
        public void ToDateFormat_FormatControlForNullable_ShouldReturnPreDetirmened()
        {
            DateTime? timeConst = new DateTime(2023, 2, 1, 14, 3, 4);
            var expected = "01.02.2023 14:03:04";

            var rslt = timeConst.ToDateFormat();

            Assert.Equal(expected, rslt);
        }

        [Fact]
        public void TextToHtml_NewLineConversion_NewLineShpuldBeHtlmBr()
        {
            string data = "This is new line\n";
            var rslt = data.TextToHtml();

            Assert.Contains("<br/>", rslt);
        }

        [Fact]
        public void ToStringRaw_StreamToString_GetTheTargetedText()
        {
            string org = "wqerğüşiöç!'^+%&/(";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(org));
            var rslt = ms.ToStringRaw();

            Assert.Equal(org, rslt);
        }

        [Fact]
        public void ToByteArray_StreamToByteArray_CastingStreamToArray()
        {
            string orgStr = "wqerğüşiöç!'^+%&/(";
            var orgByte = Encoding.UTF8.GetBytes(orgStr);
            var ms = new MemoryStream(orgByte);
            var rslt = ms.ToByteArray();

            for (int i = 0; i < orgByte.Length; i++)
            {
                byte expected = orgByte[i];
                byte actual = rslt[i];

                Assert.True(expected == actual,
                    String.Format("Expected: '{0}', Actual: '{1}' at offset {2}.", (byte)expected, (byte)actual, i)
                );
            }
        }
    }
}
