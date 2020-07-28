using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KubernetesClient.Models;
using KubernetesClient.Requests;
using KubernetesClient.Responses;
using YamlDotNet.Serialization;

namespace KubernetesClient
{
    public partial class Kubernetes
    {
        public async ValueTask<V1JobList> GetJobsAsync(string ns = "")
        {
            var res = await GetJobsManifestAsync(ns).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<HttpResponse<V1JobList>> GetJobsManifestAsync(string ns = "")
        {
            var res = string.IsNullOrEmpty(ns)
                ? await GetApiAsync($"/apis/batch/v1/jobs", "application/yaml").ConfigureAwait(false)
                : await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", "application/yaml").ConfigureAwait(false);
            var jobs = JsonSerializer.Deserialize<V1JobList>(res.Content);
            return new HttpResponse<V1JobList>(jobs)
            {
                Response = res.HttpResponseMessage,
            };
        }

        public async ValueTask<V1Job> GetJobAsync(string ns, string name)
        {
            var res = await GetJobManifestAsync(ns, name).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<HttpResponse<V1Job>> GetJobManifestAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{name}", "application/yaml").ConfigureAwait(false);
            var job = JsonSerializer.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }

        public async ValueTask<V1WatchEvent> WatchJobsAsync(string ns, string resourceVersion)
        {
            var res = await WatchJobsManifestAsync(ns, resourceVersion).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<HttpResponse<V1WatchEvent>> WatchJobsManifestAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs?watch=1&resourceVersion={resourceVersion}", "application/json").ConfigureAwait(false);
            var watch = JsonSerializer.Deserialize<V1WatchEvent>(res.Content);
            return new HttpResponse<V1WatchEvent>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }

        public async ValueTask<V1Job> CreateOrReplaceJobAsync(string ns, string yaml, string contentType)
        {
            var res = await CreateOrReplaceJobHttpAsync(ns, yaml, contentType).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<HttpResponse<V1Job>> CreateOrReplaceJobHttpAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentJobsRes = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs");
            var current = JsonSerializer.Deserialize<V1JobList>(currentJobsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{request.metadata.name}", yaml, contentType).ConfigureAwait(false);
                var job = JsonSerializer.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", yaml, contentType).ConfigureAwait(false);
                var job = JsonSerializer.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }

        public async Task<V1Job> DeleteJobAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = await DeleteJobHttpAsync(ns, name, options).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<HttpResponse<V1Job>> DeleteJobHttpAsync(string @namespace, string name, V1DeleteOptions options)
        {
            var res = await DeleteApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs/{name}", options).ConfigureAwait(false);
            var job = JsonSerializer.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }
    }
}
