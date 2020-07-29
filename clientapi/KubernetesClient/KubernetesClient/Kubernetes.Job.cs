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
        public async ValueTask<V1JobList> GetJobsAsync(string ns = "")
        {
            using var res = await GetJobsHttpAsync(ns).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Job> GetJobAsync(string ns, string name)
        {
            using var res = await GetJobHttpAsync(ns, name).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent> WatchJobsAsync(string ns, string resourceVersion)
        {
            using var res = await WatchJobsHttpAsync(ns, resourceVersion).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Job> CreateOrReplaceJobAsync(string ns, string yaml, string contentType)
        {
            using var res = await CreateOrReplaceJobHttpAsync(ns, yaml, contentType).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Job> DeleteJobAsync(string ns, string name, V1DeleteOptions options)
        {
            using var res = await DeleteJobHttpAsync(ns, name, options).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1JobList>> GetJobsHttpAsync(string ns = "", bool watch = false)
        {
            var url = string.IsNullOrEmpty(ns)
                ? $"/apis/batch/v1/jobs"
                : $"/apis/batch/v1/namespaces/{ns}/jobs";
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
            var jobs = JsonConvert.Deserialize<V1JobList>(res.Content);
            return new HttpResponse<V1JobList>(jobs)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Job>> GetJobHttpAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{name}", "application/yaml").ConfigureAwait(false);
            var job = JsonConvert.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent>> WatchJobsHttpAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs?watch=1&resourceVersion={resourceVersion}").ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent>(res.Content);
            return new HttpResponse<V1WatchEvent>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Job>> CreateOrReplaceJobHttpAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentJobsRes = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs");
            var current = JsonConvert.Deserialize<V1JobList>(currentJobsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{request.metadata.name}", yaml, contentType).ConfigureAwait(false);
                var job = JsonConvert.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", yaml, contentType).ConfigureAwait(false);
                var job = JsonConvert.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Job>> DeleteJobHttpAsync(string @namespace, string name, V1DeleteOptions options)
        {
            var res = await DeleteApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs/{name}", options).ConfigureAwait(false);
            var job = JsonConvert.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
