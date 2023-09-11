using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace KubeLife.BlazorApp.Pages
{
    public partial class Index
    {
        private List<KubeCronJobModelView> crnJobItems;
        private DateTime LastUpdateTime = DateTime.Now;
        private System.Threading.Timer? timer;
        private const int refreshTimeMs = 10 * 1000;

        protected override async Task OnInitializedAsync()
        {
            await CallCronJobs();

            await CreateTimer();
        }

        private async Task CallCronJobs()
        {
            var tmpCrns = await domainService.GetCronJobs();
            crnJobItems = tmpCrns.Result;
            LastUpdateTime = DateTime.Now;
        }

        private async Task CreateTimer()
        {
            timer = new System.Threading.Timer(async (object? stateInfo) =>
            {
                await CallCronJobs();
                await InvokeAsync(() =>
                {
                    this.StateHasChanged();
                });
            }, new System.Threading.AutoResetEvent(false), refreshTimeMs, refreshTimeMs);
        }

        private async Task NavigateToLogView(string cronJobName, bool isPodLog)
        {
            var tmp = crnJobItems.FirstOrDefault(x => x.CronJobName == cronJobName);
            var latestJob = tmp.JobDetails.OrderByDescending(x => x.StartTime).FirstOrDefault();

            var queryParams = isPodLog
                                ? await GetPodLogQueryString(latestJob)
                                : await GetBuildLogQueryString(latestJob);

            NavManager.NavigateTo(QueryHelpers.AddQueryString("LogViewer", queryParams));
        }

        internal async Task<Dictionary<string, string>> GetBuildLogQueryString(KubeJobModel latestJob)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["BuildName"] = latestJob.OwnerCronJobName,
                ["Namespace"] = latestJob.KubeNamespace,
                ["UserMsg"] = ""
            };

            return queryParams;
        }

        internal async Task<Dictionary<string, string>> GetPodLogQueryString(KubeJobModel latestJob)
        {
            var podOfJob = await domainService.GetPodofJob(latestJob.KubeNamespace, latestJob.JobUniqueName);
            var queryParams = new Dictionary<string, string>
            {
                ["PodNames"] = podOfJob.IsSuccess ? podOfJob.Result.PodName : "",
                ["Namespace"] = latestJob.KubeNamespace,
                ["UserMsg"] = podOfJob.Message
            };

            return queryParams;
        }

        public async Task NavigateToCronTimeView(string cronTiming)
        {
            string url = $"https://crontab.guru/#{cronTiming.Replace(" ", "_")}";
            await jsRuntime.InvokeVoidAsync("open", url, "_blank");
        }

        public async Task TriggerBuild(string nameSpaceParam, string buildName)
        {
            var result = await domainService.TriggerBuild(nameSpaceParam, buildName);
            string message = result.IsSuccess ? $"Successfully Triggered : {result.Result.BuildName}" : $"Error : {result.Message}";
            if (result.IsSuccess)
            {
                var indx = crnJobItems.FindIndex(x => x.CronJobName == buildName && x.Namespace == nameSpaceParam);
                if (indx > -1)
                    crnJobItems[indx].LastBuild.StatusPhase = "Running";
            }
            await jsRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
