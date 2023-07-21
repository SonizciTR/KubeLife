using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using NCrontab;
using KubeLife.Core.Extensions;
using System.Collections.Generic;

namespace KubeLife.Domain
{
    public class KubernetesDomain : IKubernetesDomain
    {
        public KubernetesDomain(IKubeService kubeService, IMapper mapper)
        {
            this.kubeService = kubeService;
            this.mapper = mapper;
        }

        private readonly IKubeService kubeService;
        private readonly IMapper mapper;
        private const string KeyFilterName = "AdvancedAnalytic";

        public async Task<List<KubeCronJobModelView>> GetCronJobs()
        {
            var crnJobsSource = await kubeService.GetCronJobs(KeyFilterName);
            var target = mapper.Map<List<KubeCronJobModelView>>(crnJobsSource);

            var jobDetails = new Dictionary<string, List<KubeJobModel>>();
            foreach (var itm in target)
            {
                string tmpKey = itm.Namespace;
                List<KubeJobModel> tmpDetail;
                if (jobDetails.ContainsKey(tmpKey))
                    tmpDetail = jobDetails[tmpKey];
                else
                {
                    tmpDetail = await kubeService.GetJobsbyNamespace(tmpKey);
                    jobDetails.Add(tmpKey, tmpDetail);
                }   

                itm.IsJobDetailSet = true;
                itm.JobDetails = tmpDetail;
            }

            target = AddingNextRunTime(target);

            return target;
        }

        internal List<KubeCronJobModelView> AddingNextRunTime(List<KubeCronJobModelView> data)
        {
            foreach (var itm in data)
            {
                var schedule = CrontabSchedule.Parse(itm.TimingRaw);
                itm.NextRunTime = schedule.GetNextOccurrence(DateTime.Now);
            }

            return data;
        }

        public async Task<KubeLifeResult<KubePodModel>> GetPodofJob(string kubeNamespace, string jobName)
        {
            var allPods = await kubeService.GetPodsofNamespace(kubeNamespace);
            var foundPod = allPods.FirstOrDefault(x => x.OwnerName == jobName);
            if (foundPod == null) return new KubeLifeResult<KubePodModel>(false, $"Pod could not found on cluster");

            return new KubeLifeResult<KubePodModel>(foundPod);
        }

        public async Task<KubeLogModel> GetLogOfPod(string kubeNamespace, string podName)
        {
            var log = await kubeService.GetLogofPod(kubeNamespace, podName);
            if (log == null) return new KubeLogModel(false, "No log found of Pod");

            return new KubeLogModel(kubeNamespace, podName, log);
        }

        public async Task<List<KubeLogModel>> GetLogOfAllPods(string kubeNamespace, List<string> podNames)
        {
            var allLogs = new List<KubeLogModel>();
            foreach (var podName in podNames)
            {
                var log = await kubeService.GetLogofPod(kubeNamespace, podName);
                if (string.IsNullOrWhiteSpace(log))
                {
                    allLogs.Add(new KubeLogModel(false, "No log found of Pod"));
                    continue;
                }

                allLogs.Add(new KubeLogModel(kubeNamespace, podName, log));
            }

            return allLogs;
        }


        public async Task<KubeLifeResult<KubeBuildModel>> TriggerBuild(string namespaceParameter, string buildConfigName)
        {
            return await kubeService.TriggerBuildConfig(namespaceParameter, buildConfigName);
        }

        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster()
        {
            return await kubeService.GetAllRoutes(500, KeyFilterName);
        }

        public async Task<List<KubePodModel>> GetPodsOfRoute(string namepsaceParam, string routeName)
        {
            var serviceData = await kubeService.GetServiceOfRoute(namepsaceParam, routeName);

            return await kubeService.GetPodsOfService(serviceData);
        }
    }
}