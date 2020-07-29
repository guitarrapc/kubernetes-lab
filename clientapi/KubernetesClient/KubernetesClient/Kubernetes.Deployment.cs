using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KubernetesClient.Models;
using KubernetesClient.Requests;
using KubernetesClient.Responses;
using KubernetesClient.Serializers;
using YamlDotNet.Serialization;

namespace KubernetesClient
{
    public partial class Kubernetes
    {
        public async ValueTask<V1DeploymentList> GetDeploymentsAsync(string ns, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await GetDeploymentsHttpAsync(ns, false, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Deployment> GetDeploymentAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await GetDeploymentHttpAsync(ns, name, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent<V1Deployment>> WatchDeploymentsAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await GetDeploymentsAsync(ns);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            var res = await WatchDeploymentsHttpAsync(ns, resourceVersion, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Deployment> CreateOrReplaceDeploymentAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await CreateOrReplaceDeploymentHttpAsync(ns, yaml, contentType, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Status> DeleteDeploymentAsync(string ns, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await DeleteDeploymentHttpAsync(ns, name, options, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1DeploymentList>> GetDeploymentsHttpAsync(string ns = "", bool watch = false, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            // build query
            var query = new StringBuilder();
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var url = string.IsNullOrEmpty(ns)
                ? $"/apis/apps/v1/deployments"
                : $"/apis/apps/v1/namespaces/{ns}/deployments";
            var res = await GetApiAsync(url, query).ConfigureAwait(false);
            var deployments = JsonConvert.Deserialize<V1DeploymentList>(res.Content);
            return new HttpResponse<V1DeploymentList>(deployments)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Deployment>> GetDeploymentHttpAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            // build query
            var query = new StringBuilder();
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var res = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", query).ConfigureAwait(false);
            var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
            return new HttpResponse<V1Deployment>(deployment)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent<V1Deployment>>> WatchDeploymentsHttpAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            // build query
            var query = new StringBuilder();
            AddQueryParameter(query, "watch", "true");
            if (!string.IsNullOrEmpty(resourceVersion))
            {
                AddQueryParameter(query, "resourceVersion", resourceVersion);
            }
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var res = await GetStreamApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", query).ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent<V1Deployment>>(res.Content);
            return new HttpResponse<V1WatchEvent<V1Deployment>>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Deployment>> CreateOrReplaceDeploymentHttpAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            // build query
            var query = new StringBuilder();
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            // gets
            var currentDeploymentsRes = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", query);
            var current = JsonConvert.Deserialize<V1DeploymentList>(currentDeploymentsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{request.metadata.name}", query, yaml, contentType).ConfigureAwait(false);
                var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
                return new HttpResponse<V1Deployment>(deployment)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", query, yaml, contentType).ConfigureAwait(false);
                var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
                return new HttpResponse<V1Deployment>(deployment)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Status>> DeleteDeploymentHttpAsync(string ns, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            // build query
            var query = new StringBuilder();
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var res = options == null
                ? await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", query).ConfigureAwait(false)
                : await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", query, options).ConfigureAwait(false);
            var status = JsonConvert.Deserialize<V1Status>(res.Content);
            return new HttpResponse<V1Status>(status)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
