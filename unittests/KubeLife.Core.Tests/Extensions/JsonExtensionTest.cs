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

        [Fact]
        public void ToModel_JsonToModelConversion_CreateAModelThenConvertToJsonAndGetModel()
        {
            var modelOrg = DummyJsonFake.Generate();
            var jsn = modelOrg.ToJson();
            var modelConverted = jsn.ToModel<DummyJsonFake>();

            Assert.Equal(modelOrg.ToJson(), modelConverted.ToJson());
        }

        [Fact]
        public void GetNodeValueAsString_GetPropValueWithKey_GetModelsPropertValuebyKey()
        {
            var modelOrg = DummyJsonFake.Generate();
            var jsn = modelOrg.ToJson();
            var orgValue = modelOrg.Name;

            var targetValue = jsn.GetNodeValueAsString("Name");

            Assert.Equal(orgValue, targetValue);
        }

        [Fact]
        public void DeepCopyJson_CopyTarget_ChangeShouldNotAffectOrginalModel()
        {
            var modelOrg = DummyJsonFake.Generate();
            var modelCopied = modelOrg.DeepCopyJson();
            modelCopied.Name = "ThisShouldChangeOnly";

            Assert.NotEqual(modelOrg.Name, modelCopied.Name);
        }
    }
}
