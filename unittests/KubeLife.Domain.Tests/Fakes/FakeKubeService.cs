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
    internal class FakeKubeService : IKubeService
    {
        public async Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null)
        {
            var jobs = new List<KubeCronJobModel>();
            for (int i = 0; i < 10; i++)
            {
                var jobitm = new KubeCronJobModel
                {
                    LastStartTime = DateTime.Now,
                    CronJobName = $"UnitTestJobName - {i + 1}",
                    IsSuspended = false ,
                    LastEndTime = DateTime.Now,
                    Namespace = $"UnitTestNamespace",
                    TimingRaw = "5 4 * * *"
                };

                jobs.Add(jobitm);
            }

            return jobs;
        }

        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutes(int routeCount = 500, string filterbyLabel = null)
        {
            return new KubeLifeResult<List<KubeRouteModel>>(true, "Unit test error");
        }

        public Task<List<KubeServiceModel>> GetAllServices(string namespaceParam)
        {
            throw new NotImplementedException();
        }

        public async Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace, string jobName)
        {
            var target = new List<KubeJobModel>();

            for (int i = 0; i < 3; i++)
            {
                var itm = new KubeJobModel
                {
                    EndTime = DateTime.Now,
                    IsSuccess = true,
                    JobUniqueName = $"UnitTest-{kubeNamespace}-Job-{i+1}" ,
                    KubeNamespace = $"UnitTestNamespace",
                    StartTime = DateTime.Now,
                    OwnerCronJobName = $"UnitTestJobName - {i + 1}",
                };
                target.Add(itm);
            }
            return target;
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

        public async Task<KubeLifeResult<List<KubeBuildModel>>> GetAllBuildsOfBuildConfig(string namespacePrm, string buildConfig)
        {
            var result = new KubeLifeResult<List<KubeBuildModel>>();
            return result;
        }

        public Task<KubeLifeResult<string>> GetLogOfBuild(string namespacePrm, string buildConfig)
        {
            throw new NotImplementedException();
        }

        public Task<KubeLifeResult<string>> CreateJobFromCronJob(string namespacePrm, string cronJobName, string newJobUnqName)
        {
            throw new NotImplementedException();
        }
    }
}
