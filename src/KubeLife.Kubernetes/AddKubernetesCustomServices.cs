using KubeLife.Core.Logging;
using KubeLife.Kubernetes.Models;
using KubeLife.Kubernetes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes
{
    public static class AddKubernetesCustomServices
    {
        public static void AddKubernetesServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IKubeRestService, KubeRestService>();         
        }
    }
}
