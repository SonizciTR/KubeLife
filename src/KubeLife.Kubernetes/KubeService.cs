using k8s;
using k8s.Models;
using KubeLife.Kubernetes.Extensions;
using KubeLife.Kubernetes.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using KubeLife.Core.Extensions;
using KubeLife.Kubernetes.Services;
using KubeLife.Core.Models;
using System.Xml.Linq;

namespace KubeLife.Kubernetes
{
    /// <summary>
    /// Establishes communication with Kubernetes APIs
    /// </summary>
    public class KubeService : IKubeService
    {
        private readonly IKubeRestService restService;
        public KubeService(KubeConfigModel settings, IKubeRestService kubeRestService)
        {
            Settings = settings;
            restService = kubeRestService ?? new KubeRestService(Settings);
        }

        public KubeConfigModel Settings { get; }

        private k8s.Kubernetes GetKubeClient()
        {
            var config = new KubernetesClientConfiguration
            {
                SkipTlsVerify = true,
            };

            config.Host = Settings.KubeServerUrl;
            if (!string.IsNullOrEmpty(Settings.KubeAccessToken))
                config.AccessToken = Settings.KubeAccessToken;
            else
            {
                config.Password = Settings.KubePassWord;
                config.Username = Settings.KubeUserName;
            }

            var client = new k8s.Kubernetes(config);

            return client;
        }

        /// <summary>
        /// Gives the CronJobs Information
        /// </summary>
        /// <param name="filterbyLabel">Filters the with label match</param>
        /// <param name="includeJobDetails">Makes extra query for CronJobs Detail</param>
        /// <returns>CronJobs detail</returns>
        public async Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null)
        {
            using k8s.Kubernetes client = GetKubeClient();

            var allCronnJobs = await client.ListCronJobForAllNamespacesAsync();
            var tmpCrns = filterbyLabel == null ? allCronnJobs : allCronnJobs.WhereLabelContains(filterbyLabel);

            return tmpCrns.ToKubeCronJobModelList();
        }

        /// <summary>
        /// Get the Jobs Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <param name="cronJobName">Filters the jobs by Cronjob name</param>
        /// <returns>Jobs detail</returns>
        public async Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace, string cronJobName)
        {
            using k8s.Kubernetes client = GetKubeClient();
            var selector_val = $"job-name={cronJobName}";
            //var selector_val = $"metadata.ownerReferences[0].name={cronJobName}";
            //var selector_val = $"name={cronJobName}";
            //var selector_val = $"metadata.app=cronjob-retrain-score";

            var jbs = await client.ListNamespacedJobAsync(kubeNamespace);
            //var jbsFiltered = await client.ListNamespacedJobAsync(kubeNamespace, labelSelector: selector_val);
            
            var allJobs = jbs.ToKubeJobModelList();
            var jobs = allJobs.Where(x => x.OwnerCronJobName == cronJobName).OrderByDescending(y => y.StartTime).ToList();
            return jobs;
        }

        /// <summary>
        /// Gets the Pod's Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Pods Information</returns>
        public async Task<List<KubePodModel>> GetPodsofNamespace(string kubeNamespace)
        {
            using k8s.Kubernetes client = GetKubeClient();
            var pods = await client.ListNamespacedPodAsync(kubeNamespace);

            return pods.ToKubePodModelList();
        }

        /// <summary>
        /// Gives the pod's terminal log as string
        /// </summary>
        /// <param name="kubeNamespace">Pod's namespace</param>
        /// <param name="podName">Pod name</param>
        /// <returns>Terminal log as string</returns>
        public async Task<string> GetLogofPod(string kubeNamespace, string podName)
        {
            using k8s.Kubernetes client = GetKubeClient();
            try
            {
                var logStream = await client.ReadNamespacedPodLogAsync(podName, kubeNamespace);

                return logStream.ToStringForm();
            }
            catch (k8s.Autorest.HttpOperationException exKube)
            {
                return exKube.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }

        /// <summary>
        /// Triggring a Build from BuildConfig Template
        /// </summary>
        /// <param name="namespaceParameter">Project name</param>
        /// <param name="buildConfigName">BuildConfig name</param>
        /// <returns>Build details</returns>
        public async Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName)
        {
            return await restService.TriggerBuildConfig(namespaceParameter, buildConfigName);
        }

        /// <summary>
        /// Getting all routes available for cluster
        /// </summary>
        /// <param name="routeCount">Max route count query</param>
        /// <param name="filterbyLabel">label name to filter</param>
        /// <returns></returns>
        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutes(int routeCount = 500, string filterbyLabel = null)
        {
            return await restService.GetAllRoutesForCluster(routeCount, filterbyLabel);
        }

        /// <summary>
        /// List all services of project
        /// </summary>
        /// <param name="namespaceParam">project name</param>
        /// <returns>service info list</returns>
        public async Task<List<KubeServiceModel>> GetAllServices(string namespaceParam)
        {
            using k8s.Kubernetes client = GetKubeClient();

            var rawServices = await client.ListNamespacedServiceAsync(namespaceParam);

            return rawServices.ToKubeServiceModelList();
        }

        /// <summary>
        /// Finds the Route's services
        /// </summary>
        /// <param name="namespaceParam">project name</param>
        /// <param name="routeName">route name</param>
        /// <returns></returns>
        public async Task<KubeServiceModel> GetServiceOfRoute(string namespaceParam, string routeName)
        {
            var serviceRoute = await restService.GetRouteByNamespace(namespaceParam, routeName);
            if (!serviceRoute.IsSuccess) return null;

            using k8s.Kubernetes client = GetKubeClient();

            var rawServices = await client.ListNamespacedServiceAsync(namespaceParam);

            var allServices = rawServices.ToKubeServiceModelList();

            return allServices?.FirstOrDefault(x => x.ServiceName == serviceRoute.Result.ServiceName);
        }

        /// <summary>
        /// Gets the pods that mapped to given service
        /// </summary>
        /// <param name="serviceInfo">service name</param>
        /// <returns>list of pod info</returns>
        public async Task<List<KubePodModel>> GetPodsOfService(KubeServiceModel serviceInfo)
        {
            var allPods = await GetPodsofNamespace(serviceInfo.Namespace);

            var filterdPods = allPods.Where(
                x => x.Labels.Any(y => serviceInfo.Selector.ContainsKey(y.Key) && serviceInfo.Selector[y.Key] == y.Value)
                ).ToList();

            return filterdPods;
        }

        /// <summary>
        /// List all builds from specific BuildConfig
        /// </summary>
        /// <param name="namespacePrm">namespace</param>
        /// <param name="buildConfig">name of BuildConfig</param>
        /// <returns></returns>
        public async Task<KubeLifeResult<List<KubeBuildModel>>> GetAllBuildsOfBuildConfig(string namespacePrm, string buildConfig)
        {
            return await restService.GetAllBuildsOfBuildConfig(namespacePrm, buildConfig);
        }

        /// <summary>
        /// Returns the log of build
        /// </summary>
        /// <param name="namespacePrm">kube project name</param>
        /// <param name="buildConfig">build name </param>
        /// <returns></returns>
        public async Task<KubeLifeResult<string>> GetLogOfBuild(string namespacePrm, string buildConfig)
        {
            return await restService.GetLogOfBuild(namespacePrm, buildConfig);
        }

        /// <summary>
        /// Creates job from given Cron Job
        /// </summary>
        /// <param name="namespacePrm">Kubernetes namespace</param>
        /// <param name="cronJobName">Template of Job</param>
        /// <param name="newJobUnqName">Unique name given to the job</param>
        /// <returns></returns>
        public async Task<KubeLifeResult<string>> CreateJobFromCronJob(string namespacePrm, string cronJobName, string newJobUnqName)
        {
            var client = GetKubeClient();

            var annoJob = new Dictionary<string, string>()
            {
                {  "cronjob.kubernetes.io/instantiate", "manual" },
            };

            var cron_job = await client.BatchV1.ReadNamespacedCronJobAsync(cronJobName, namespacePrm);
            var ownRefs = new List<V1OwnerReference> 
            { 
                new V1OwnerReference(apiVersion: "batch/v1", kind:"CronJob", name:cronJobName, uid: cron_job.Uid())
            };
            var job = new V1Job(apiVersion: "batch/v1", kind: "Job",
                metadata: new V1ObjectMeta(name: newJobUnqName, annotations: annoJob, ownerReferences: ownRefs),                
                spec: cron_job.Spec.JobTemplate.Spec
            );

            var rslt = await client.BatchV1.CreateNamespacedJobAsync(job, namespacePrm);
            if (rslt != null)
            {
                return new KubeLifeResult<string>(true, "Job created successfully.", cronJobName);
            }

            return new KubeLifeResult<string>(false, "Job could not created form CronJob.");
        }
    }
}