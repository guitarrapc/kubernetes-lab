using System.Text.Json;
using System.Threading.Tasks;
using KubernetesClient;
using KubernetesClient.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KubernetesApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KubernetesManifestController : ControllerBase
    {
        private ILogger<KubernetesController> _logger;
        private Kubernetes _kubernetes;

        public KubernetesManifestController(Kubernetes kubernetes, ILogger<KubernetesController> logger)
        {
            _kubernetes = kubernetes;
            _logger = logger;
        }

        #region deployments

        // curl localhost:5000/kubernetesmanifest/deployments
        // response format: YAML
        [HttpGet("deployments")]
        public async Task<string> GetDeployments()
        {
            _logger.LogInformation("Get deployments manifest api.");
            var res = await _kubernetes.GetApiAsync("/apis/apps/v1/deployments", "application/yaml");
            return res;
        }

        // curl "localhost:5000/kubernetesmanifest/deployment?namespace=default&name=kubernetesapisample"
        // curl "localhost:5000/kubernetesmanifest/deployment?namespace=default&name=kubernetesapisample&watch=true"
        // response format: YAML
        [HttpGet("deployment")]
        public async Task<string> GetDeployment(string @namespace, string name, bool watch = false, string resourceVersion = "")
        {
            if (watch)
            {
                if (string.IsNullOrEmpty(resourceVersion))
                {
                    var res = await _kubernetes.GetApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments", "application/json");
                    var deployments = JsonSerializer.Deserialize<V1MetadataOnly>(res);
                    resourceVersion = deployments.metadata.resourceVersion;
                }
                _logger.LogInformation($"Watch deployment api. resourceVersion {resourceVersion}");
                var watchRes = await _kubernetes.GetStreamApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments?watch=1&resourceVersion={resourceVersion}", "application/yaml");
                return watchRes;
            }
            else
            {
                _logger.LogInformation("Get deployment api.");
                var res = await _kubernetes.GetApiAsync($"/apis/apps/v1/namespaces/{@namespace}/deployments/{name}", "application/yaml");
                return res;
            }
        }
        #endregion

        #region jobs

        // curl localhost:5000/kubernetesmanifest/jobs
        // response format: YAML
        [HttpGet("jobs")]
        public async Task<string> GetJobs()
        {
            _logger.LogInformation("Get jobs manifest api.");
            var res = await _kubernetes.GetApiAsync("/apis/batch/v1/jobs", "application/yaml");
            return res;
        }

        // curl "localhost:5000/kubernetesmanifest/job?namespace=default&name=hoge"
        // curl "localhost:5000/kubernetesmanifest/job?namespace=default&name=hoge&watch=true"
        // response format: YAML
        [HttpGet("job")]
        public async Task<string> GetJob(string @namespace, string name, bool watch = false, string resourceVersion = "")
        {
            if (watch)
            {
                if (string.IsNullOrEmpty(resourceVersion))
                {
                    var res = await _kubernetes.GetApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs", "application/json");
                    var deployments = JsonSerializer.Deserialize<V1MetadataOnly>(res);
                    resourceVersion = deployments.metadata.resourceVersion;
                }
                _logger.LogInformation($"Watch job api. resourceVersion {resourceVersion}");
                var watchRes = await _kubernetes.GetStreamApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs?watch=1&resourceVersion={resourceVersion}", "application/yaml");
                return watchRes;
            }
            else
            {
                _logger.LogInformation("Get job api.");
                var res = await _kubernetes.GetApiAsync($"/apis/batch/v1/namespaces/{@namespace}/jobs/{name}", "application/yaml");
                return res;
            }
        }
        #endregion
    }
}