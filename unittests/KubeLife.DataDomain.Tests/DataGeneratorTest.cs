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
        public void DataGenerator_DataAddingtoEmptyClass_FilliningEachProperty()
        {
            var data = DataGenerator.GenerateData<TestClass1>();
            Assert.NotNull(data.Name);
        }
    }
}