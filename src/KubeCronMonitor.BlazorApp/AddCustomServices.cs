using KubeCronMonitor.Kubernetes;
using KubeCronMonitor.Kubernetes.Models;
using Microsoft.Extensions.Configuration;

namespace KubeCronMonitor.BlazorApp
{
    public static class AddCustomServices
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            var kubeSetting = GetFromConfig(config);

            services.AddSingleton<IKubeService>(new KubeService(kubeSetting));
        }

        private static KubeConfigModel GetFromConfig(IConfiguration config)
        {
            var target = new KubeConfigModel();
            target.ServerUrl = config.GetValue<string>("KubernetesSetting:ServerUrl");
            target.AccessToken = config.GetValue<string>("KubernetesSetting:AccessToken");
            target.UserName = config.GetValue<string>("KubernetesSetting:UserName");
            target.PassWord = config.GetValue<string>("KubernetesSetting:PassWord");
            return target;
        }
    }
}
