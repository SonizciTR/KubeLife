using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Core.Logging;
using KubeLife.Data.Services;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using KubeLife.Kubernetes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KubeLife.Domain
{
    public static class AddDomainCustomServices
    {
        public static void AddDomainServices(this IServiceCollection services, IConfiguration config)
        {
            var kubeSetting = GetFromConfig(config);

            services.AddSingleton<IKubeLogger, KubeConsoleLogger>();

            services.AddSingleton<KubeConfigModel>(kubeSetting);
            //services.AddSingleton<IKubeService>(new KubeService(kubeSetting, null, null));
            services.AddSingleton<IKubeService, KubeService>();
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
            target.KubeServerUrl = config.GetValue<string>("KubeServerUrl");
            target.KubeAccessToken = config.GetValue<string>("KubeAccessToken");
            target.KubeUserName = config.GetValue<string>("KubeUserName");
            target.KubePassWord = config.GetValue<string>("KubePassWord");

            target.S3ModelAccessKey = config.GetValue<string>("S3ModelAccessKey");
            target.S3ModelEndpoint = config.GetValue<string>("S3ModelEndpoint");
            target.S3ModelSecretKey = config.GetValue<string>("S3ModelSecretKey");

            return target;
        }
    }
}
