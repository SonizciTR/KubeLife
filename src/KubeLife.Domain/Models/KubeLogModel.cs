using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Models
{
    public class KubeLogModel
    {
        public KubeLogModel()
        {   
        }

        public KubeLogModel(bool isFound, string errorMessage)
        {
            this.IsLogFound = isFound;
            this.ErrorMessage = errorMessage;
        }

        public KubeLogModel(string kubeNamespace, string podName, string log)
        {
            IsLogFound = true;
            this.Namespace = kubeNamespace;
            PodName = podName;
            this.LogText = log;
        }

        public bool IsLogFound { get; set; }
        public string PodName { get; set; }
        public string Namespace { get; set; }
        public string LogText { get; set; }
        public string LogTextHtml => LogText.Replace("\n", "<br />");
        public string ErrorMessage { get; set; }
    }
}
