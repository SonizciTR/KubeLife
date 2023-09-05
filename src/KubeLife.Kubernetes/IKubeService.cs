using KubeLife.Core.Models;
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

        /// <summary>
        /// Triggring a Build from BuildConfig Template
        /// </summary>
        /// <param name="namespaceParameter">Project name</param>
        /// <param name="buildConfigName">BuildConfig name</param>
        /// <returns>Build details</returns>
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName);

        /// <summary>
        /// Getting all routes available for cluster
        /// </summary>
        /// <param name="routeCount">Max route count query</param>
        /// <param name="filterbyLabel">label name to filter</param>
        /// <returns></returns>
        Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutes(int routeCount = 500, string filterbyLabel = null);

        /// <summary>
        /// List all services of project
        /// </summary>
        /// <param name="namespaceParam">project name</param>
        /// <returns>service info list</returns>
        Task<List<KubeServiceModel>> GetAllServices(string namespaceParam);

        /// <summary>
        /// Finds the Route's services
        /// </summary>
        /// <param name="namespaceParam">project name</param>
        /// <param name="routeName">route name</param>
        /// <returns></returns>
        Task<KubeServiceModel> GetServiceOfRoute(string namespaceParam, string routeName);

        /// <summary>
        /// Gets the pods that mapped to given service
        /// </summary>
        /// <param name="serviceInfo">service name</param>
        /// <returns>list of pod info</returns>
        Task<List<KubePodModel>> GetPodsOfService(KubeServiceModel serviceInfo);

        /// <summary>
        /// List all builds from specific BuildConfig
        /// </summary>
        /// <param name="namespacePrm">namespace</param>
        /// <param name="buildConfig">name of BuildConfig</param>
        /// <returns></returns>
        Task<KubeLifeResult<List<KubeBuildModel>>> GetAllBuildsOfBuildConfig(string namespacePrm, string buildConfig);

        /// <summary>
        /// Returns the log of build
        /// </summary>
        /// <param name="namespacePrm">kube project name</param>
        /// <param name="buildConfig">build name </param>
        /// <returns></returns>
        Task<KubeLifeResult<string>> GetLogOfBuild(string namespacePrm, string buildConfig);
    }
}