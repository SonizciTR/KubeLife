using KubeLife.Core.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Tests.Fakes
{
    internal class FakeSuccessEmptyKubeService : IKubeService
    {
        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutes(int routeCount = 500, string filterbyLabel = null)
        {
            return new KubeLifeResult<List<KubeRouteModel>>(true, "Unit test error");
        }

        public Task<List<KubeServiceModel>> GetAllServices(string namespaceParam)
        {
            throw new NotImplementedException();
        }

        public Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLogofPod(string kubeNamespace, string podName)
        {
            throw new NotImplementedException();
        }

        public Task<List<KubePodModel>> GetPodsofNamespace(string kubeNamespace)
        {
            throw new NotImplementedException();
        }

        public Task<List<KubePodModel>> GetPodsOfService(KubeServiceModel serviceInfo)
        {
            throw new NotImplementedException();
        }

        public Task<KubeServiceModel> GetServiceOfRoute(string namespaceParam, string routeName)
        {
            throw new NotImplementedException();
        }

        public Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName)
        {
            throw new NotImplementedException();
        }
    }
}
