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
        public async Task GetCronJobs_ThereIsNoData_EmptyReturn()
        {
            var service = GetSerivce();
            var result = await service.GetCronJobs();

            Assert.True(result.IsSuccess);
            Assert.True(result.Result.Any());
            Assert.True(result.Result[0].JobDetails.Any());
        }
    }
}