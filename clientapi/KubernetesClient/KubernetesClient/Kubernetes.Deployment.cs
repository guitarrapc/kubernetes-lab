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
        public async ValueTask<V1DeploymentList> GetDeploymentsAsync(string ns, bool watch = false)
        {
            var res = await GetDeploymentsHttpAsync(ns, watch).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Deployment> GetDeploymentAsync(string ns, string name)
        {
            var res = await GetDeploymentHttpAsync(ns, name).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1WatchEvent> WatchDeploymentsAsync(string ns, string resourceVersion)
        {
            var res = await WatchDeploymentsHttpAsync(ns, resourceVersion).ConfigureAwait(false);
            return res.Body;
        }
        public async ValueTask<V1Deployment> CreateOrReplaceDeploymentAsync(string ns, string yaml, string contentType)
        {
            var res = await CreateOrReplaceDeploymentHttpAsync(ns, yaml, contentType).ConfigureAwait(false);
            return res.Body;
        }
        public async Task<V1Status> DeleteDeploymentAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = await DeleteDeploymentHttpAsync(ns, name, options).ConfigureAwait(false);
            return res.Body;
        }

        #region http
        public async ValueTask<HttpResponse<V1DeploymentList>> GetDeploymentsHttpAsync(string ns = "", bool watch = false)
        {
            var url = string.IsNullOrEmpty(ns)
                ? $"/apis/apps/v1/deployments"
                : $"/apis/apps/v1/namespaces/{ns}/deployments";
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
            var deployments = JsonConvert.Deserialize<V1DeploymentList>(res.Content);
            return new HttpResponse<V1DeploymentList>(deployments)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Deployment>> GetDeploymentHttpAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}").ConfigureAwait(false);
            var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
            return new HttpResponse<V1Deployment>(deployment)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1WatchEvent>> WatchDeploymentsHttpAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments?watch=true&resourceVersion={resourceVersion}").ConfigureAwait(false);
            var watch = JsonConvert.Deserialize<V1WatchEvent>(res.Content);
            return new HttpResponse<V1WatchEvent>(watch)
            {
                Response = res.HttpResponseMessage,
            };
        }
        public async ValueTask<HttpResponse<V1Deployment>> CreateOrReplaceDeploymentHttpAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentDeploymentsRes = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments");
            var current = JsonConvert.Deserialize<V1DeploymentList>(currentDeploymentsRes.Content);

            if (current.items.Any(x => x.metadata.name == request.metadata.name))
            {
                // replace
                var res = await PutApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{request.metadata.name}", yaml, contentType).ConfigureAwait(false);
                var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
                return new HttpResponse<V1Deployment>(deployment)
                {
                    Response = res.HttpResponseMessage,
                };
            }
            else
            {
                // create
                var res = await PostApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", yaml, contentType).ConfigureAwait(false);
                var deployment = JsonConvert.Deserialize<V1Deployment>(res.Content);
                return new HttpResponse<V1Deployment>(deployment)
                {
                    Response = res.HttpResponseMessage,
                };
            }
        }
        public async Task<HttpResponse<V1Status>> DeleteDeploymentHttpAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = options == null
                ? await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}").ConfigureAwait(false)
                : await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", options).ConfigureAwait(false);
            var status = JsonConvert.Deserialize<V1Status>(res.Content);
            return new HttpResponse<V1Status>(status)
            {
                Response = res.HttpResponseMessage,
            };
        }
        #endregion
    }
}
