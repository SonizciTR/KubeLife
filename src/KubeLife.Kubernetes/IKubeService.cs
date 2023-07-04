﻿using KubeLife.Kubernetes.Models;

namespace KubeLife.Kubernetes
{
    /// <summary>
    /// Establishes communication with Kubernetes APIs
    /// </summary>
    public interface IKubeService
    {
        /// <summary>
        /// Gives the CronJobs Information
        /// </summary>
        /// <param name="filterbyLabel">Filters the with label match</param>
        /// <param name="includeJobDetails">Makes extra query for CronJobs Detail</param>
        /// <returns>CronJobs detail</returns>
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null, bool includeJobDetails = true);

        /// <summary>
        /// Get the Jobs Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Jobs detail</returns>
        Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace);

        /// <summary>
        /// Gets the Pods Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Pods Information</returns>
        Task<List<KubePodModel>> GetPodsofNamespace(string kubeNamespace);
    }
}