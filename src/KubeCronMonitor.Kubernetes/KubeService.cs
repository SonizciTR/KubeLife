using k8s;
using KubeCronMonitor.Kubernetes.Extensions;
using KubeCronMonitor.Kubernetes.Models;

namespace KubeCronMonitor.Kubernetes
{
    public class KubeService : IKubeService
    {
        public KubeService(KubeConfigModel settings)
        {
            Settings = settings;
        }

        public KubeConfigModel Settings { get; }

        public async Task<List<KubeCronJobModel>> GetCronJobs()
        {
            var config = new KubernetesClientConfiguration();
            config.Host = Settings.ServerUrl;
            if (!string.IsNullOrEmpty(Settings.AccessToken))
                config.AccessToken = Settings.AccessToken;
            else
            {
                config.Password = Settings.PassWord;
                config.Username = Settings.UserName;
            }

            var client = new k8s.Kubernetes(config);

            var crnJobs = await client.ListCronJobForAllNamespacesAsync();
            return crnJobs.ToKubeCronJobModelList();
        }
    }
}