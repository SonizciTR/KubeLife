using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.Components;

namespace KubeLife.BlazorApp.Components
{
    public partial class BuildImageShow
    {
        //[Parameter]
        //public string StatusPhase { get; set; }
        //[Parameter]
        //public string BuildName { get; set; }
        [Parameter]
        public KubeBuildModel BuildData { get; set; }
    }
}
