using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KubeLife.Core.Extensions;
using KubeLife.Core.Tests.Extensions.Mocks;

namespace KubeLife.Core.Tests.Extensions
{
    public class JsonExtensionTest
    {
        [Fact]
        public void ToJson_ModelToJsonString_ShoulGetSimpleString()
        {
            string expected = "{\"Id\":1,\"Name\":\"Test\"}";
            var model = DummyJsonFake.Generate();

            var rslt = model.ToJson();

            Assert.Equal(expected, rslt);
        }
    }
}
