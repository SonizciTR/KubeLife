using k8s;
using k8s.Models;
using KubeCronMonitor.Kubernetes.Extensions;
using KubeCronMonitor.Kubernetes.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KubeCronMonitor.Kubernetes
{
    public class KubeService : IKubeService
    {
        public KubeService(KubeConfigModel settings)
        {
            Settings = settings;
        }

        public KubeConfigModel Settings { get; }

        private k8s.Kubernetes GetKubeClient()
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
            return client;
        }

        public async Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null, bool includeJobDetails = true)
        {
            using k8s.Kubernetes client = GetKubeClient();

            var crnJobs = await client.ListCronJobForAllNamespacesAsync();
            var jobDetails = new Dictionary<string, List<KubeJobModel>>();
            if (includeJobDetails)
            {
                foreach (var itm in crnJobs.Items)
                {
                    string tmpKey = itm.Metadata.NamespaceProperty;
                    if (jobDetails.ContainsKey(tmpKey))
                        continue;

                    var tmpData = await GetJobsbyNamespace(tmpKey);
                    jobDetails.Add(tmpKey, tmpData);
                }
            }

            //var tmp = filterbyLabel == null ? crnJobs : crnJobs.Items.Where(x =>
            //{
            //    return x.Labels.Any(y => true);
            //});

            return crnJobs.ToKubeCronJobModelList(jobDetails);
        }

        public async Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace)
        {
            using k8s.Kubernetes client = GetKubeClient();
            var jbs = await client.ListNamespacedJobAsync(kubeNamespace);
            return jbs.ToKubeJobModelList();
        }
    }
}