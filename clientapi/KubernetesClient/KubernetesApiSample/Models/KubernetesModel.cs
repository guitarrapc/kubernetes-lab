using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KubernetesClient;
using KubernetesClient.Requests;
using YamlDotNet.Serialization;

namespace KubernetesApiSample.Models
{
    public class KubernetesModel
    {
        public async ValueTask<string> CreateOrReplaceDeploymentAsync(KubernetesApi kubeapi, KubernetesDeploymentCreateOrUpdateRequest request)
        {
            var deploymentsJson = await kubeapi.GetApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments", "application/json");
            var deployments = JsonSerializer.Deserialize<KubernetesDeployments>(deploymentsJson);

            // decode body base64
            var decodedBody = KubernetesApi.Base64ToString(request.Body);
            var yamlDeserializer = new DeserializerBuilder().Build();
            var deploymentRequest = yamlDeserializer.Deserialize<KubernetesDeploymentMetadata>(decodedBody);

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
    }
}
