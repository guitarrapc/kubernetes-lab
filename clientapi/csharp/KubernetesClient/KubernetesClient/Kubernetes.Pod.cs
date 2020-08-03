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
using YamlDotNet.Serialization.NamingConventions;

namespace KubernetesClient
{
    public partial class Kubernetes
    {
        public async ValueTask<V1PodList> GetPodsAsync(string ns, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await GetPodsHttpAsync(ns, false, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Pod> GetPodAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await GetPodHttpAsync(ns, name, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent<V1Pod>> WatchPodsAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await GetPodsAsync(ns);
                resourceVersion = deployments.Metadata.ResourceVersion;
            }
            var res = await WatchPodsHttpAsync(ns, resourceVersion, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Pod> CreateOrReplacePodAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await CreateOrReplacePodHttpAsync(ns, yaml, contentType, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Pod> DeletePodAsync(string ns, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var res = await DeletePodHttpAsync(ns, name, options, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1PodList>> GetPodsHttpAsync(string ns = "", bool watch = false, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            // build query
            var query = new StringBuilder();
            if (watch)
            {
                AddQueryParameter(query, "watch", "true");
            }
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var url = string.IsNullOrEmpty(ns)
                ? $"/api/v1/pods"
                : $"/api/v1/namespaces/{ns}/pods";
            var res = await GetApiAsync(url, query).ConfigureAwait(false);
            var Pods = JsonConvert.Deserialize<V1PodList>(res.Content);
            return new HttpResponse<V1PodList>(Pods)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Pod>> GetPodHttpAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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

            var res = await GetApiAsync($"/api/v1/namespaces/{ns}/pods/{name}", query).ConfigureAwait(false);
            var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
            return new HttpResponse<V1Pod>(Pod)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent<V1Pod>>> WatchPodsHttpAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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

            var res = await GetStreamApiAsync($"/api/v1/namespaces/{ns}/pods", query).ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent<V1Pod>>(res.Content);
            return new HttpResponse<V1WatchEvent<V1Pod>>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Pod>> CreateOrReplacePodHttpAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
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
            var currentPodsRes = await GetApiAsync($"/api/v1/namespaces/{ns}/pods", query);
            var current = JsonConvert.Deserialize<V1PodList>(currentPodsRes.Content);

            if (current.Items.Any(x => x.Metadata.Name == request.Metadata.Name))
            {
                // replace
                var res = await PutApiAsync($"/api/v1/namespaces/{ns}/pods/{request.Metadata.Name}", query, yaml, contentType).ConfigureAwait(false);
                var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
                return new HttpResponse<V1Pod>(Pod)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/api/v1/namespaces/{ns}/pods", query, yaml, contentType).ConfigureAwait(false);
                var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
                return new HttpResponse<V1Pod>(Pod)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Pod>> DeletePodHttpAsync(string ns, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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
                ? await DeleteApiAsync($"/api/v1/namespaces/{ns}/pods/{name}", query).ConfigureAwait(false)
                : await DeleteApiAsync($"/api/v1/namespaces/{ns}/pods/{name}", query, options).ConfigureAwait(false);
            var status = JsonConvert.Deserialize<V1Pod>(res.Content);
            return new HttpResponse<V1Pod>(status)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
