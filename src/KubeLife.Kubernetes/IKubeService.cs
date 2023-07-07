using KubeLife.Kubernetes.Models;

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
        /// <returns>CronJobs detail</returns>
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null);

        /// <summary>
        /// Get the Jobs Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Jobs detail</returns>
        Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace);
        
        /// <summary>
        /// Gives the pod's terminal log as string
        /// </summary>
        /// <param name="kubeNamespace">Pod's namespace</param>
        /// <param name="podName">Pod name</param>
        /// <returns>Terminal log as string</returns>
        Task<string> GetLogofPod(string kubeNamespace, string podName);

        /// <summary>
        /// Gets the Pods Information
        /// </summary>
        /// <param name="kubeNamespace">Filters by kubernetes namespace</param>
        /// <returns>Pods Information</returns>
        Task<List<KubePodModel>> GetPodsofNamespace(string kubeNamespace);
    }
}