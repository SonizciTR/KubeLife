using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using Microsoft.Extensions.Configuration;

namespace KubeLife.BlazorApp
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
            target.ServerUrl = config.GetValue<string>("KubeServerUrl");
            target.AccessToken = config.GetValue<string>("KubeAccessToken");
            target.UserName = config.GetValue<string>("KubeUserName");
            target.PassWord = config.GetValue<string>("KubePassWord");
            return target;
        }
    }
}
