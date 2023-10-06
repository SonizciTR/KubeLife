using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using NCrontab;
using KubeLife.Core.Extensions;
using System.Collections.Generic;
using KubeLife.Data.Services;

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

        public async Task<KubeLifeResult<List<KubeCronJobModelView>>> GetCronJobs()
        {
            var crnJobsSource = await kubeService.GetCronJobs(KeyFilterName);
            var target = mapper.Map<List<KubeCronJobModelView>>(crnJobsSource);

            target = await AddingJobDetails(target);

            target = AddingNextRunTime(target);

            target = await AddingJobLastBuild(target);

            return new KubeLifeResult<List<KubeCronJobModelView>>(target);
        }

        internal async Task<List<KubeCronJobModelView>> AddingJobLastBuild(List<KubeCronJobModelView> target)
        {
            foreach (var itm in target)
            {
                var allBuilds = await kubeService.GetAllBuildsOfBuildConfig(itm.Namespace, itm.CronJobName);
                if (!allBuilds.IsSuccess) continue;

                itm.LastBuild = allBuilds.Result[0];
            }

            return target;
        }

        internal async Task<List<KubeCronJobModelView>> AddingJobDetails(List<KubeCronJobModelView> target)
        {
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

        public async Task<KubeLifeResult<KubeLogModel>> GetLogOfPod(string kubeNamespace, string podName)
        {
            var log = await kubeService.GetLogofPod(kubeNamespace, podName);
            if (log == null) return new KubeLifeResult<KubeLogModel>(false, "No log found of Pod");

            var model = new KubeLogModel(kubeNamespace, podName, log);

            return new KubeLifeResult<KubeLogModel>(model);
        }

        public async Task<List<KubeLifeResult<KubeLogModel>>> GetLogOfAllPods(string kubeNamespace, List<string> podNames)
        {
            var allLogs = new List<KubeLifeResult<KubeLogModel>>();
            foreach (var podName in podNames)
            {
                var log = await kubeService.GetLogofPod(kubeNamespace, podName);
                if (string.IsNullOrWhiteSpace(log))
                {
                    allLogs.Add(new KubeLifeResult<KubeLogModel>(false, "No log found of Pod"));
                    continue;
                }

                allLogs.Add(new KubeLifeResult<KubeLogModel>(new KubeLogModel(kubeNamespace, podName, log)));
            }

            return allLogs;
        }

        public async Task<KubeLifeResult<KubeBuildModel>> TriggerBuild(string namespaceParameter, string buildConfigName)
        {
            return await kubeService.TriggerBuildConfig(namespaceParameter, buildConfigName);
        }

        public async Task<KubeLifeResult<List<KubeRouteViewModel>>> GetAllRoutesForCluster()
        {
            var allRoutesRaw = await kubeService.GetAllRoutes(500, KeyFilterName);
            if (!allRoutesRaw.IsSuccess) return new KubeLifeResult<List<KubeRouteViewModel>>(false, allRoutesRaw.Message);

            var allRoutes = mapper.Map<List<KubeRouteViewModel>>(allRoutesRaw.Result);

            foreach (var route in allRoutes)
            {
                var allBuilds = await kubeService.GetAllBuildsOfBuildConfig(route.Namespace, route.Name);
                if (!allBuilds.IsSuccess) continue;

                route.LastBuild = allBuilds.Result[0];
            }

            return new KubeLifeResult<List<KubeRouteViewModel>>(allRoutes);
        }

        public async Task<KubeLifeResult<List<KubePodModel>>> GetPodsOfRoute(string namepsaceParam, string routeName)
        {
            var serviceData = await kubeService.GetServiceOfRoute(namepsaceParam, routeName);

            var pods = await kubeService.GetPodsOfService(serviceData);

            return new KubeLifeResult<List<KubePodModel>>(pods);
        }

        public async Task<KubeLifeResult<string>> GetLastBuildLog(string namespacePrm, string buildConfig)
        {
            var allBuilds = await kubeService.GetAllBuildsOfBuildConfig(namespacePrm, buildConfig);
            if(!allBuilds.IsSuccess)
                return new KubeLifeResult<string>(false, allBuilds.Message);

            if (!allBuilds.Result.IsAny())
                return new KubeLifeResult<string>(false, $"No builds found for {buildConfig} at {namespacePrm}");
            
            var lst = allBuilds.Result[0];

            return await kubeService.GetLogOfBuild(namespacePrm, lst.BuildName);
        }

        public async Task<KubeLifeResult<string>> TriggerCronJob(string namespacePrm, string cronJobName)
        {
            var tmpAllJobsOfNamespace = await kubeService.GetJobsbyNamespace(namespacePrm);
            var tmpCronsJob = tmpAllJobsOfNamespace.Where(x => x.OwnerCronJobName == cronJobName)
                                                    .OrderByDescending(y => y.StartTime)
                                                    .ToList();
            if (tmpCronsJob.IsAny() && tmpCronsJob.Any(x => x.IsStillRunning))
                return new KubeLifeResult<string>(false, "There is still running job. Can not trigger new one.");
            
            string tmpJobName = $"{cronJobName}-kubelife-{DateTime.Now.ToString("yyyMMddHHmmss")}";
            var rslt = await kubeService.CreateJobFromCronJob(namespacePrm, cronJobName, tmpJobName);
            return rslt;
        }
    }
}