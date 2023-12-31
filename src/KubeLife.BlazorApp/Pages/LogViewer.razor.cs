﻿using KubeLife.BlazorApp.Extensions;
using KubeLife.Core.Models;
using KubeLife.Core.Extensions;
using KubeLife.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace KubeLife.BlazorApp.Pages
{
    public partial class LogViewer
    {
        public string PodNamesRaw { get; set; }
        public string KubeNamespace { get; set; }
        public string BuildNamesRaw { get; set; }

        public string QueryResult { get; set; } = "Loading...";
        public List<KubeLifeResult<KubeLogModel>> LogResults { get; set; }
        public string LogText { get; set; }
        public string LabelForLog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            KubeNamespace = NavManager.QueryStringSingle("Namespace");
            PodNamesRaw = NavManager.QueryStringSingle("PodNames");
            BuildNamesRaw = NavManager.QueryStringSingle("BuildName");

            var isPodLog = !string.IsNullOrEmpty(PodNamesRaw);
            var isBuildLog = !string.IsNullOrEmpty(BuildNamesRaw);

            if (string.IsNullOrEmpty(KubeNamespace)) QueryResult = "Sorry,'Namespace' information is missing.";
            else if (isPodLog) await ShowPodLog();
            else if (isBuildLog) await ShowBuildLog();
            else
            {
                string sysMsg = NavManager.QueryStringSingle("UserMsg");
                QueryResult = !string.IsNullOrEmpty(sysMsg) ? sysMsg : "Sorry, There is missing information.";
                return;
            }
        }

        public async Task ShowBuildLog()
        {
            var logInfo = await domainService.GetLastBuildLog(KubeNamespace, BuildNamesRaw);
            LabelForLog = $"{DateTime.Now} : {BuildNamesRaw}";
            LogText = "";
            QueryResult = "";

            if (!logInfo.IsSuccess)
            {
                QueryResult = logInfo.Message;
                return;
            }

            LogText = logInfo.Result.TextToHtml();
        }

        public async Task ShowPodLog()
        {
            var podNames = PodNamesRaw.Split(",").ToList();
            if (string.IsNullOrWhiteSpace(PodNamesRaw) || !podNames.IsAny())
            {
                string sysMsg = NavManager.QueryStringSingle("UserMsg");
                QueryResult = !string.IsNullOrEmpty(sysMsg) ? sysMsg : "There is no 'Job Detail' provided.";
                return;
            }

            LogResults = await domainService.GetLogOfAllPods(KubeNamespace, podNames);
            if (!LogResults.IsAny())
                QueryResult = "There is no 'POD' data found.";
            else
                await PodNameChanged(LogResults[0].Result.PodName);
        }

        public async Task PodComboBoxChanged(ChangeEventArgs e)
        {
            var tmpPodName = e.Value.ToString();
            await PodNameChanged(tmpPodName);
        }

        public async Task PodNameChanged(string podName)
        {
            LogText = "";
            QueryResult = "";
            var log = await domainService.GetLogOfPod(KubeNamespace, podName);
            if (log.IsSuccess)
                LogText = log.Result.LogText.TextToHtml();
            else
                QueryResult = log.Message;
        }
    }
}
