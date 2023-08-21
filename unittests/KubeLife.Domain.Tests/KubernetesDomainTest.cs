using AutoMapper;
using KubeLife.Domain.Tests.Fakes;

namespace KubeLife.Domain.Tests
{
    public class KubernetesDomainTest
    {
        private IKubernetesDomain GetSerivce()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            return new KubernetesDomain(new FakeKubeService(), mapper);
        }

        [Fact]
        public void Test1()
        {
            var service = GetSerivce();

            Assert.True(service != null);
        }
    }
}