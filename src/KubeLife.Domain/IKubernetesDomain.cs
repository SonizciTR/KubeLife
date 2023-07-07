﻿using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;
using KubeLife.Kubernetes.Services;

namespace KubeLife.Domain
{
    public interface IKubernetesDomain
    {
        Task<List<KubeCronJobModelView>> GetCronJobs(string filterbyLabel);
        Task<KubeLifeResult<string>> GetLogofJob(string kubeNamespace, string jobName);
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuild(string namespaceParameter, string buildConfigName);
    }
}