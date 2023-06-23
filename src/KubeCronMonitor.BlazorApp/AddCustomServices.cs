using KubeCronMonitor.Kubernetes;

namespace KubeCronMonitor.BlazorApp
{
    public static class AddCustomServices
    {
        public static void AddAppServices(this IServiceCollection services) 
        {
            string serverUrl = "";
            string accessToken = "";
            string userName = "";
            string password = "";
                

            services.AddSingleton<IKubeService>(new KubeService(serverUrl, accessToken, userName, password));
        }
    }
}
