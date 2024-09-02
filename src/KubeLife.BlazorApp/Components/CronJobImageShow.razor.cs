using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.Components;

namespace KubeLife.BlazorApp.Components
{
    public partial class CronJobImageShow
    {
        [Parameter]
        public KubeCronJobModelView CronJobDetail { get; set; }
    }
}
