using KubeLife.Core.Models;
using KubeLife.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KubeLife.Core.Extensions;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Net;

namespace KubeLife.Kubernetes.Services
{
    public class KubeBuildModel
    {
        public string BuildName { get; internal set; }
        public string Namespace { get; internal set; }
    }
    internal class KubeRestService : IKubeRestService
    {
        public KubeRestService(KubeConfigModel settings)
        {
            Settings = settings;
        }

        public KubeConfigModel Settings { get; }

        private const string BodyTriggerBuildConfig = "{\"kind\":\"BuildRequest\",\"apiVersion\":\"build.openshift.io/v1\",\"metadata\":{\"name\":\"REPLACE_NAME\", \"namespace\":\"REPLACE_PROJECT\"},\"triggeredBy\":[{\"message\":\"Manually triggered\"}],\"dockerStrategyOptions\":{},\"sourceStrategyOptions\":{}}";
        public async Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName)
        {
            //https://api.ocplab.thy.com:6443/apis/build.openshift.io/v1/namespaces/kubelife/buildconfigs/kubelifeapp/instantiate
            string url = $"{Settings.ServerUrl}/apis/build.openshift.io/v1/namespaces/{namespaceParameter}/buildconfigs/{buildConfigName}/instantiate";

            string tmpBody = BodyTriggerBuildConfig.Replace("REPLACE_NAME", buildConfigName).Replace("REPLACE_PROJECT", namespaceParameter);
            var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(tmpBody));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var handler = new HttpClientHandler(); //Openshift SSL is custom so validation fails, workaround
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.AccessToken);

            var response = await client.PostAsync(url, content);

            var respJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return new KubeLifeResult<KubeBuildModel>(false, respJson);

            var target = new KubeBuildModel();
            target.BuildName = respJson.GetNodeValueAsString("metadata", "name");
            target.Namespace = respJson.GetNodeValueAsString("metadata", "namespace");
            return new KubeLifeResult<KubeBuildModel>(target);
        }
    }
}
