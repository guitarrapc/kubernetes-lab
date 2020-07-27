using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KubernetesClient;
using KubernetesClient.Models;
using KubernetesClient.Requests;
using YamlDotNet.Serialization;

namespace KubernetesApiSample.Models
{
    public class KubernetesModel
    {
        public async ValueTask<string> CreateOrReplaceDeploymentAsync(Kubernetes kubeapi, KubernetesCreateOrUpdateRequest request)
        {
            var deploymentsJson = await kubeapi.GetApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments");
            var deployments = JsonSerializer.Deserialize<V1DeploymentList>(deploymentsJson);

            // decode body base64
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var yamlDeserializer = new DeserializerBuilder().Build();
            var deploymentRequest = yamlDeserializer.Deserialize<V1MetadataOnly>(decodedBody);

            if (deployments.items.Any(x => x.metadata.name == deploymentRequest.metadata.name))
            {
                // replace
                var res = await kubeapi.PutApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments/{deploymentRequest.metadata.name}", decodedBody, request.BodyContentType);
                return res;
            }
            else
            {
                // create
                var res = await kubeapi.PostApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments", decodedBody, request.BodyContentType);
                return res;
            }
        }

        public async ValueTask<string> CreateOrReplaceJobAsync(Kubernetes kubeapi, KubernetesCreateOrUpdateRequest request)
        {
            var deploymentsJson = await kubeapi.GetApiAsync($"/apis/batch/v1/namespaces/{request.NameSpace}/jobs");
            var deployments = JsonSerializer.Deserialize<V1JobList>(deploymentsJson);

            // decode body base64
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var yamlDeserializer = new DeserializerBuilder().Build();
            var deploymentRequest = yamlDeserializer.Deserialize<V1MetadataOnly>(decodedBody);

            if (deployments.items.Any(x => x.metadata.name == deploymentRequest.metadata.name))
            {
                // replace
                var res = await kubeapi.PutApiAsync($"/apis/batch/v1/namespaces/{request.NameSpace}/jobs/{deploymentRequest.metadata.name}", decodedBody, request.BodyContentType);
                return res;
            }
            else
            {
                // create
                var res = await kubeapi.PostApiAsync($"/apis/batch/v1/namespaces/{request.NameSpace}/jobs", decodedBody, request.BodyContentType);
                return res;
            }
        }
    }
}
