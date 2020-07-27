using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using KubernetesApiSample.Models;
using KubernetesClient;
using KubernetesClient.Models;
using KubernetesClient.Requests;
using KubernetesClient.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KubernetesApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KubernetesController : ControllerBase
    {
        private ILogger<KubernetesController> logger;
        private KubernetesApi kubeapi;

        public KubernetesController(KubernetesApi kubeapi, ILogger<KubernetesController> logger)
        {
            this.kubeapi = kubeapi;
            this.logger = logger;
        }

        // curl localhost:5000/kubernetes/status
        // response format: always json response.
        [HttpGet("status")]
        public KubernetesClientStatusResponse GetStatus()
        {
            logger.LogInformation("Get status.");
            var res = kubeapi.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/configure_client?skipcertificatevalidate=true
        // response format: always json response.
        [HttpGet("configure_client")]
        [HttpPost("configure_client")]
        public KubernetesClientStatusResponse ConfigureClient(bool skipCertificateValidate = true)
        {
            logger.LogInformation("Configure status.");
            kubeapi.ConfigureClient(skipCertificateValidate);
            var res = kubeapi.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/configure_response?isJson=true
        // response format: always json response.
        [HttpGet("configure_response")]
        [HttpPost("configure_response")]
        public KubernetesClientStatusResponse ConfigureResponse(bool isJson = false)
        {
            logger.LogInformation("Configure response.");
            kubeapi.ConfigureResponse(isJson);
            var res = kubeapi.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/spec
        // response format: always json response.
        [HttpGet("spec")]
        public async Task<string> GetSpec()
        {
            logger.LogInformation("Get spec api.");
            var res = await kubeapi.GetOpenApiSpecAsync();
            return res;
        }

        // curl localhost:5000/kubernetes/deployments
        // response format: depends on accept type
        [HttpGet("deployments")]
        public async Task<string[]> GetDeployments()
        {
            logger.LogInformation("Get deployments api.");
            var res = await kubeapi.GetApiAsync("/apis/apps/v1/deployments", "application/json");
            var deployments = JsonSerializer.Deserialize<V1DeploymentList>(res);
            return deployments.items
                .Select(item => $"{item.metadata.@namespace}/{item.metadata.name}")
                .ToArray();
        }

        // curl localhost:5000/kubernetes/deployments/manifest
        // response format: depends on accept type
        [HttpGet("deployments/manifest")]
        public async Task<string> GetDeploymentsManifest()
        {
            logger.LogInformation("Get deployments manifest api.");
            var res = await kubeapi.GetApiAsync("/apis/apps/v1/deployments");
            return res;
        }

        // curl "localhost:5000/kubernetes/deployment?namespace=default&name=kubernetesapisample"
        // response format: depends on accept type
        [HttpGet("deployment")]
        public async Task<V1Deployment> GetDeployment(string @namespace, string name)
        {
            logger.LogInformation("Get deployment api.");
            var res = await kubeapi.GetApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments/{name}", "application/json");
            var deployment = JsonSerializer.Deserialize<V1Deployment>(res);
            return deployment;
        }

        // curl "localhost:5000/kubernetes/deployment/manifest?namespace=default&name=kubernetesapisample"
        // curl "localhost:5000/kubernetes/deployment/manifest?namespace=default&name=kubernetesapisample&watch=true"
        // response format: depends on accept type
        [HttpGet("deployment/manifest")]
        public async Task<string> GetDeploymentManifest(string @namespace, string name, bool watch = false, string resourceVersion = "")
        {
            if (watch)
            {
                if (string.IsNullOrEmpty(resourceVersion))
                {
                    var res = await kubeapi.GetApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments", "application/json");
                    var deployments = JsonSerializer.Deserialize<V1DeploymentMetadataOnly>(res);
                    resourceVersion = deployments.metadata.resourceVersion;
                }
                logger.LogInformation($"Watch deployment api. resourceVersion {resourceVersion}");
                var watchRes = await kubeapi.GetStreamApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments?watch=1&resourceVersion={resourceVersion}", "application/json");
                return watchRes;
            }
            else
            {
                logger.LogInformation("Get deployment api.");
                var res = await kubeapi.GetApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments/{name}");
                return res;
            }
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogYXBwcy92MSAjICBmb3IgazhzIHZlcnNpb25zIGJlZm9yZSAxLjkuMCB1c2UgYXBwcy92MWJldGEyICBhbmQgYmVmb3JlIDEuOC4wIHVzZSBleHRlbnNpb25zL3YxYmV0YTENCmtpbmQ6IERlcGxveW1lbnQNCm1ldGFkYXRhOg0KICBuYW1lOiBmcm9udGVuZA0Kc3BlYzoNCiAgc2VsZWN0b3I6DQogICAgbWF0Y2hMYWJlbHM6DQogICAgICBhcHA6IGd1ZXN0Ym9vaw0KICB0ZW1wbGF0ZToNCiAgICBtZXRhZGF0YToNCiAgICAgIGxhYmVsczoNCiAgICAgICAgYXBwOiBndWVzdGJvb2sNCiAgICBzcGVjOg0KICAgICAgY29udGFpbmVyczoNCiAgICAgICAgLSBuYW1lOiBwaHAtcmVkaXMNCiAgICAgICAgICBpbWFnZTogazhzLmdjci5pby9ndWVzdGJvb2s6djMNCiAgICAgICAgICByZXNvdXJjZXM6DQogICAgICAgICAgICByZXF1ZXN0czoNCiAgICAgICAgICAgICAgY3B1OiAxMDBtDQogICAgICAgICAgICAgIG1lbW9yeTogMTAwTWkNCiAgICAgICAgICAgIGxpbWl0czoNCiAgICAgICAgICAgICAgY3B1OiAyMDAwbQ0KICAgICAgICAgICAgICBtZW1vcnk6IDEwMDBNaQ0KICAgICAgICAgIGVudjoNCiAgICAgICAgICAgIC0gbmFtZTogR0VUX0hPU1RTX0ZST00NCiAgICAgICAgICAgICAgdmFsdWU6IGRucw0KICAgICAgICAgICAgICAjIElmIHlvdXIgY2x1c3RlciBjb25maWcgZG9lcyBub3QgaW5jbHVkZSBhIGRucyBzZXJ2aWNlLCB0aGVuIHRvDQogICAgICAgICAgICAgICMgaW5zdGVhZCBhY2Nlc3MgZW52aXJvbm1lbnQgdmFyaWFibGVzIHRvIGZpbmQgc2VydmljZSBob3N0DQogICAgICAgICAgICAgICMgaW5mbywgY29tbWVudCBvdXQgdGhlICd2YWx1ZTogZG5zJyBsaW5lIGFib3ZlLCBhbmQgdW5jb21tZW50IHRoZQ0KICAgICAgICAgICAgICAjIGxpbmUgYmVsb3c6DQogICAgICAgICAgICAgICMgdmFsdWU6IGVudg0KICAgICAgICAgIHBvcnRzOg0KICAgICAgICAgICAgLSBuYW1lOiBodHRwLXNlcnZlcg0KICAgICAgICAgICAgICBjb250YWluZXJQb3J0OiAzMDAwDQo="}'
        // response format: depends on accept type
        [HttpPost("deployment")]
        public async Task<string> CreateOrUpdateDeployment(KubernetesDeploymentCreateOrUpdateRequest request)
        {
            logger.LogInformation($"Create or Replace deployment api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}, body {request.Body}");
            var model = new KubernetesModel();
            return await model.CreateOrReplaceDeploymentAsync(kubeapi, request);
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "name": "frontend"}'
        // response format: depends on accept type
        [HttpDelete("deployment")]
        public async Task<string> DeleteDeployment(KubernetesDeploymentDeleteRequest request)            
        {
            logger.LogInformation($"Delete deployment api. namespace {request.NameSpace}, name {request.Name}");
            var res = await kubeapi.DeleteApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments/{request.Name}");
            return res;
        }
    }
}