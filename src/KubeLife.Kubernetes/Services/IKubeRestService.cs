﻿using KubeLife.Core.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Kubernetes.Services
{
    public interface IKubeRestService
    {
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName);
        Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster(int routeCount = 500, string filterbyLabel = null);
        Task<KubeLifeResult<KubeRouteModel>> GetRouteByNamespace(string namespacePrm, string serviceName);
        Task<KubeLifeResult<List<KubeBuildModel>>> GetAllBuildsOfBuildConfig(string namepspacePrm, string buildConfig);
    }
}