using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KubernetesClient.Models;
using KubernetesClient.Requests;
using YamlDotNet.Serialization;

namespace KubernetesClient
{
    public partial class Kubernetes
    {
        public async ValueTask<V1JobList> GetJobsAsync(string ns = "")
        {
            var res = string.IsNullOrEmpty(ns)
                ? await GetApiAsync($"/apis/batch/v1/jobs").ConfigureAwait(false)
                : await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs").ConfigureAwait(false);
            var jobs = JsonSerializer.Deserialize<V1JobList>(res);
            return jobs;
        }
        public async ValueTask<string> GetJobsManifestAsync(string ns = "")
        {
            var res = string.IsNullOrEmpty(ns)
                ? await GetApiAsync($"/apis/batch/v1/jobs", "application/yaml").ConfigureAwait(false)
                : await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", "application/yaml").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<V1Job> GetJobAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{name}").ConfigureAwait(false);
            var job = JsonSerializer.Deserialize<V1Job>(res);
            return job;
        }
        public async ValueTask<string> GetJobManifestAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{name}", "application/yaml").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<V1WatchEvent> WatchJobsAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs?watch=1&resourceVersion={resourceVersion}", "application/json").ConfigureAwait(false);
            var watch = JsonSerializer.Deserialize<V1WatchEvent>(res);
            return watch;
        }

        public async ValueTask<string> WatchJobsManifestAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs?watch=1&resourceVersion={resourceVersion}", "application/json").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<V1Job> CreateOrReplaceJobAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentJobsRes = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs");
            var current = JsonSerializer.Deserialize<V1JobList>(currentJobsRes);

            var res = current.items.Any(x => x.metadata.name == request.metadata.name)
                // replace
                ? await PutApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{request.metadata.name}", yaml, contentType).ConfigureAwait(false)
                // create
                : await PostApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", yaml, contentType).ConfigureAwait(false);
            var job = JsonSerializer.Deserialize<V1Job>(res);
            return job;
        }

        public async Task<V1Job> DeleteJobAsync(string @namespace, string name, V1DeleteOptions options)
        {
            var res = await DeleteApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs/{name}", options).ConfigureAwait(false);
            var status = JsonSerializer.Deserialize<V1Job>(res);
            return status;
        }
    }
}
