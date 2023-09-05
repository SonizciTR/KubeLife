using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.Components;

namespace KubeLife.BlazorApp.Components
{
    public partial class BuildImageShow
    {
        [Parameter]
        public KubeBuildModel BuildData { get; set; }
    }
}
