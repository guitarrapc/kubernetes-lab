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
        public async ValueTask<V1DeploymentList> GetDeploymentsAsync(string ns)
        {
            var res = string.IsNullOrEmpty(ns)
                ? await GetApiAsync($"/apis/apps/v1/deployments").ConfigureAwait(false)
                : await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments").ConfigureAwait(false);
            var deployments = JsonSerializer.Deserialize<V1DeploymentList>(res);
            return deployments;
        }
        public async ValueTask<string> GetDeploymentsManifestAsync(string ns)
        {
            var res = string.IsNullOrEmpty(ns)
                ? await GetApiAsync($"/apis/apps/v1/deployments", "application/yaml").ConfigureAwait(false)
                : await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", "application/yaml").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<V1Deployment> GetDeploymentAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}").ConfigureAwait(false);
            var deployment = JsonSerializer.Deserialize<V1Deployment>(res);
            return deployment;
        }
        public async ValueTask<string> GetDeploymentManifestAsync(string ns, string name)
        {
            var res = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", "application/yaml").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<string> WatchDeploymentsManifestAsync(string ns, string resourceVersion)
        {
            var res = await GetStreamApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments?watch=1&resourceVersion={resourceVersion}", "application/json").ConfigureAwait(false);
            return res;
        }

        public async ValueTask<V1Deployment> CreateOrReplaceDeploymentAsync(string ns, string yaml, string contentType)
        {
            var yamlDeserializer = new DeserializerBuilder().Build();
            var request = yamlDeserializer.Deserialize<V1MetadataOnly>(yaml);

            var currentDeploymentsRes = await GetApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments");
            var current = JsonSerializer.Deserialize<V1DeploymentList>(currentDeploymentsRes);

            var res = current.items.Any(x => x.metadata.name == request.metadata.name)
                // replace
                ? await PutApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{request.metadata.name}", yaml, contentType).ConfigureAwait(false)
                // create
                : await PostApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments", yaml, contentType).ConfigureAwait(false);
            var deployment = JsonSerializer.Deserialize<V1Deployment>(res);
            return deployment;
        }

        public async Task<V1Status> DeleteDeploymentAsync(string ns, string name, V1DeleteOptions options)
        {
            var res = options == null
                ? await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}").ConfigureAwait(false)
                : await DeleteApiAsync($"/apis/apps/v1/namespaces/{ns}/deployments/{name}", options).ConfigureAwait(false);
            var status = JsonSerializer.Deserialize<V1Status>(res);
            return status;
        }
    }
}
