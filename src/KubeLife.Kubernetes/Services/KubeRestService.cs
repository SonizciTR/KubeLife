﻿using KubeLife.Core.Models;
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
using KubeLife.Kubernetes.Models.Routes;
using KubeLife.Kubernetes.Extensions;
using KubeLife.Kubernetes.Models.Service;

namespace KubeLife.Kubernetes.Services
{
    internal class KubeRestService : IKubeRestService
    {
        public KubeRestService(KubeConfigModel settings)
        {
            Settings = settings;
        }

        public KubeConfigModel Settings { get; }

        private async Task<HttpResponseMessage?> CallApi(string url, bool isPostCall, string body)
        {
            var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(body));

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

            return await (isPostCall ? client.PostAsync(url, content) : client.GetAsync(url));
        }

        private const string BodyTriggerBuildConfig = "{\"kind\":\"BuildRequest\",\"apiVersion\":\"build.openshift.io/v1\",\"metadata\":{\"name\":\"REPLACE_NAME\", \"namespace\":\"REPLACE_PROJECT\"},\"triggeredBy\":[{\"message\":\"Manually triggered\"}],\"dockerStrategyOptions\":{},\"sourceStrategyOptions\":{}}";
        public async Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName)
        {
            //https://[Server adress and port]/apis/build.openshift.io/v1/namespaces/kubelife/buildconfigs/kubelifeapp/instantiate
            string url = $"{Settings.ServerUrl}/apis/build.openshift.io/v1/namespaces/{namespaceParameter}/buildconfigs/{buildConfigName}/instantiate";

            string tmpBody = BodyTriggerBuildConfig.Replace("REPLACE_NAME", buildConfigName).Replace("REPLACE_PROJECT", namespaceParameter);
            var response = await CallApi(url, true, tmpBody);

            var respJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return new KubeLifeResult<KubeBuildModel>(false, respJson);

            var target = new KubeBuildModel();
            target.BuildName = respJson.GetNodeValueAsString("metadata", "name");
            target.Namespace = respJson.GetNodeValueAsString("metadata", "namespace");
            return new KubeLifeResult<KubeBuildModel>(target);
        }

        public async Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster(int routeCount = 500, string filterbyLabel = null)
        {
            //https://[Server adress and port]/apis/route.openshift.io/v1/routes?limit=500
            string url = $"{Settings.ServerUrl}/apis/route.openshift.io/v1/routes?limit={500}";
            var response = await CallApi(url, false, "");

            var respJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return new KubeLifeResult<List<KubeRouteModel>>(false, respJson);

            var modelRaw = respJson.ToModel<KubeCustomObjectforRoute>();
            return new KubeLifeResult<List<KubeRouteModel>>(modelRaw.ToKubeRouteModelList(filterbyLabel));
        }

        public async Task<KubeLifeResult<KubeServiceModel>> GetRoutesService(string namespacePrm, string serviceName)
        {
            //https:/[Server adress and port]/apis/route.openshift.io/v1/namespaces/standy-prod/routes/streamlit-standby
            string url = $"{Settings.ServerUrl}/apis/route.openshift.io/v1/namespaces/{namespacePrm}/routes/{serviceName}";
            var response = await CallApi(url, false, "");

            var respJson = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return new KubeLifeResult<KubeServiceModel>(false, respJson);

            var modelRaw = respJson.ToModel<KubeCustomObjectforService>();
            return new KubeLifeResult<KubeServiceModel>(modelRaw.ToKubeServiceModel());
        }
    }
}
