using System;
using System.Collections.Generic;
using System.Linq;
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
        public async ValueTask<V1PodList> GetPodsAsync(string ns, bool watch = false)
        {
            var res = await GetPodsHttpAsync(ns, watch).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Pod> GetPodAsync(string ns, string name)
        {
            var res = await GetPodHttpAsync(ns, name).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent> WatchPodsAsync(string ns, string resourceVersion)
        {
            var res = await WatchPodsHttpAsync(ns, resourceVersion).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Pod> CreateOrReplacePodAsync(string ns, string yaml, string contentType)
        {
            var res = await CreateOrReplacePodHttpAsync(ns, yaml, contentType).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Pod> DeletePodAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = await DeletePodHttpAsync(ns, name, options).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1PodList>> GetPodsHttpAsync(string ns = "", bool watch = false)
        {
            var url = string.IsNullOrEmpty(ns)
                ? $"/api/v1/pods"
                : $"/api/v1/namespaces/{ns}/pods";
            var queryParameters = new List<string>();
            if (watch)
            {
                queryParameters.Add($"watch=true");
            }
            if (queryParameters.Count > 0)
            {
                url += "?" + string.Join("&", queryParameters);
            }
            var res = await GetApiAsync(url).ConfigureAwait(false);
            var Pods = JsonConvert.Deserialize<V1PodList>(res.Content);
            return new HttpResponse<V1PodList>(Pods)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Pod>> GetPodHttpAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/api/v1/namespaces/{ns}/pods/{name}").ConfigureAwait(false);
            var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
            return new HttpResponse<V1Pod>(Pod)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent>> WatchPodsHttpAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/api/v1/namespaces/{ns}/pods?watch=true&resourceVersion={resourceVersion}").ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent>(res.Content);
            return new HttpResponse<V1WatchEvent>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Pod>> CreateOrReplacePodHttpAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentPodsRes = await GetApiAsync($"/api/v1/namespaces/{ns}/pods");
            var current = JsonConvert.Deserialize<V1PodList>(currentPodsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/api/v1/namespaces/{ns}/pods/{request.metadata.name}", yaml, contentType).ConfigureAwait(false);
                var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
                return new HttpResponse<V1Pod>(Pod)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/api/v1/namespaces/{ns}/pods", yaml, contentType).ConfigureAwait(false);
                var Pod = JsonConvert.Deserialize<V1Pod>(res.Content);
                return new HttpResponse<V1Pod>(Pod)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Pod>> DeletePodHttpAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = options == null
                ? await DeleteApiAsync($"/api/v1/namespaces/{ns}/pods/{name}").ConfigureAwait(false)
                : await DeleteApiAsync($"/api/v1/namespaces/{ns}/pods/{name}", options).ConfigureAwait(false);
            var status = JsonConvert.Deserialize<V1Pod>(res.Content);
            return new HttpResponse<V1Pod>(status)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
