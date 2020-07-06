using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubernetesClient;
using KubernetesClient.Requests;
using KubernetesClient.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KubernetesApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KuberntesController : ControllerBase
    {
        private ILogger<KuberntesController> logger;
        private KubernetesApi kubeapi;

        public KuberntesController(KubernetesApi kubeapi, ILogger<KuberntesController> logger)
        {
            this.kubeapi = kubeapi;
            this.logger = logger;
        }

        // curl localhost:5000/kuberntes/status
        // response format: always json response.
        [HttpGet("status")]
        public KubernetesClientStatusResponse GetStatus()
        {
            logger.LogInformation("Get status.");
            var res = kubeapi.GetStatusAsync();
            return res;
        }

        // curl localhost:5000/kuberntes/configure_client?skipcertificatevalidate=true
        // response format: always json response.
        [HttpGet("configure_client")]
        [HttpPost("configure_client")]
        public KubernetesClientStatusResponse ConfigureClient(bool skipCertificateValidate = true)
        {
            logger.LogInformation("Configure status.");
            kubeapi.ConfigureClient(skipCertificateValidate);
            var res = kubeapi.GetStatusAsync();
            return res;
        }

        // curl localhost:5000/kuberntes/spec
        // response format: always json response.
        [HttpGet("spec")]
        public async Task<string> GetSpec()
        {
            logger.LogInformation("Get spec api.");
            var res = await kubeapi.GetOpenApiSpecAsync();
            return res;
        }

        // curl localhost:5000/Kuberntes/deployments
        // response format: depends on accept type
        [HttpGet("deployments")]
        public async Task<string> GetDeployments()
        {
            logger.LogInformation("Get deployments api.");
            var res = await kubeapi.GetApiAsync("/apis/apps/v1/deployments");
            return res;
        }

        // curl -X POST localhost:5000/kuberntes/deployments --data '{"namespace": "hoge", "name":"frontend", "body": @deployments.yaml}'
        // curl -X POST -H "Content-Type: application/json" localhost:5000/kuberntes/deployments --data '{"namespace": "hoge", "name":"frontend"}'
        // response format: depends on accept type
        [HttpPost("deployment")]
        public async Task<string> PostDeployment(KubernetesDeploymentCreateRequest request)
        {
            logger.LogInformation("Post deployments api.");
            var body = Utf8Json.JsonSerializer.ToJsonString<KubernetesDeploymentBody>(request.Body);
            var res = await kubeapi.PostApiAsync($"/apis/apps/v1/namespaces/{request.NameSpace}/deployments/{request.Name}", body, "application/yaml");
            return res;
        }
    }
}