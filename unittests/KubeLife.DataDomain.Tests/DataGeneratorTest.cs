using KubeLife.Data.S3;
using KubeLife.DataCenter.Helpers;
using NuGet.Frameworks;

namespace KubeLife.DataDomain.Tests
{
    public class TestClass1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class DataGeneratorTest
    {
        

        [Fact]
        public async Task DataGenerator_DataAddingtoEmptyClass_FilliningEachProperty()
        {
            var data = DataGenerator.GenerateData<TestClass1>();
            Assert.NotNull(data.Name);
        }

        [Fact]
        public void DataGenerator_GettingDataAsList_FilliningEachProperty()
        {
            int cnt = 10;
            var data = DataGenerator.GenerateData<TestClass1>(cnt);
            Assert.Equal(cnt, data.Count);
            Assert.True(data.All(x => !string.IsNullOrWhiteSpace(x.Name)));
        }
    }
}