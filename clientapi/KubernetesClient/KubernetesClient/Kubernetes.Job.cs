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
        public async ValueTask<V1JobList> GetJobsAsync(string ns = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            using var res = await GetJobsHttpAsync(ns, false, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Job> GetJobAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            using var res = await GetJobHttpAsync(ns, name, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent<V1Job>> WatchJobsAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await GetDeploymentsAsync(ns);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            using var res = await WatchJobsHttpAsync(ns, resourceVersion, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Job> CreateOrReplaceJobAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            using var res = await CreateOrReplaceJobHttpAsync(ns, yaml, contentType, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Job> DeleteJobAsync(string ns, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            using var res = await DeleteJobHttpAsync(ns, name, options, labelSelectorParameter, timeoutSecondsParameter).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1JobList>> GetJobsHttpAsync(string ns = "", bool watch = false, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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
                ? $"/apis/batch/v1/jobs"
                : $"/apis/batch/v1/namespaces/{ns}/jobs";
            var res = await GetApiAsync(url, query).ConfigureAwait(false);
            var jobs = JsonConvert.Deserialize<V1JobList>(res.Content);
            return new HttpResponse<V1JobList>(jobs)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Job>> GetJobHttpAsync(string ns, string name, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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

            var res = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{name}", query, "application/yaml").ConfigureAwait(false);
            var job = JsonConvert.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent<V1Job>>> WatchJobsHttpAsync(string ns, string resourceVersion = "", string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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

            var res = await GetStreamApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", query).ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent<V1Job>>(res.Content);
            return new HttpResponse<V1WatchEvent<V1Job>>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Job>> CreateOrReplaceJobHttpAsync(string ns, string yaml, string contentType, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
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
            var currentJobsRes = await GetApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", query);
            var current = JsonConvert.Deserialize<V1JobList>(currentJobsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs/{request.metadata.name}", query, yaml, contentType).ConfigureAwait(false);
                var job = JsonConvert.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/apis/batch/v1/namespaces/{ns}/jobs", query, yaml, contentType).ConfigureAwait(false);
                var job = JsonConvert.Deserialize<V1Job>(res.Content);
                return new HttpResponse<V1Job>(job)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Job>> DeleteJobHttpAsync(string @namespace, string name, V1DeleteOptions options, string labelSelectorParameter = null, int? timeoutSecondsParameter = null)
        {
            var query = new StringBuilder();
            if (!string.IsNullOrEmpty(labelSelectorParameter))
            {
                AddQueryParameter(query, "labelSelector", labelSelectorParameter);
            }
            if (timeoutSecondsParameter != null)
            {
                AddQueryParameter(query, "timeoutSeconds", timeoutSecondsParameter.Value.ToString());
            }

            var res = await DeleteApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs/{name}", query, options).ConfigureAwait(false);
            var job = JsonConvert.Deserialize<V1Job>(res.Content);
            return new HttpResponse<V1Job>(job)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
