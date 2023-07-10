﻿using k8s;
using k8s.Models;
using KubeLife.Kubernetes.Extensions;
using KubeLife.Kubernetes.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using KubeLife.Core.Extensions;
using KubeLife.Kubernetes.Services;
using KubeLife.Core.Models;

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

            var tmpx = await GetAllRoutes(500, filterbyLabel);

            return tmpCrns.ToKubeCronJobModelList();
        }

        /// <summary>
        /// Get the Jobs Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Jobs detail</returns>
        public async Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace)
        {
            using k8s.Kubernetes client = GetKubeClient();
            var jbs = await client.ListNamespacedJobAsync(kubeNamespace);
            return jbs.ToKubeJobModelList();
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
            catch(k8s.Autorest.HttpOperationException exKube)
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

        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutes(int routeCount = 500, string filterbyLabel = null)
        {
            return await restService.GetAllRoutesForCluster(routeCount, filterbyLabel);
        }
    }
}