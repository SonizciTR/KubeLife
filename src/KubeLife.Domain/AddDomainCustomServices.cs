using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Data.Services;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KubeLife.Domain
{
    public static class AddDomainCustomServices
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            var kubeSetting = GetFromConfig(config);

            services.AddSingleton<IKubeService>(new KubeService(kubeSetting, null));
            services.AddSingleton<IKubeS3Factory, KubeS3Factory>();
            services.AddSingleton<IKubernetesDomain, KubernetesDomain>();
            services.AddSingleton<IDataDomain, DataDomain>();

            AddAutoMapper(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static KubeConfigModel GetFromConfig(IConfiguration config)
        {
            var target = new KubeConfigModel();
            target.ServerUrl = config.GetValue<string>("KubeServerUrl");
            target.AccessToken = config.GetValue<string>("KubeAccessToken");
            target.UserName = config.GetValue<string>("KubeUserName");
            target.PassWord = config.GetValue<string>("KubePassWord");
            return target;
        }
    }
}
