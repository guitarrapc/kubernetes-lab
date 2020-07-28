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
        private readonly ILogger<KubernetesController> _logger;
        private readonly Kubernetes _operations;

        public KubernetesManifestController(Kubernetes operations, ILogger<KubernetesController> logger)
        {
            _operations = operations;
            _logger = logger;
        }

        #region deployments

        // curl localhost:5000/kubernetesmanifest/deployments
        // curl localhost:5000/kubernetesmanifest/deployments?ns=default
        // response format: YAML
        [HttpGet("deployments")]
        public async Task<string> GetDeployments(string ns = "")
        {
            _logger.LogInformation("Get deployments manifest api.");
            var res = await _operations.GetDeploymentsManifestAsync(ns);
            return res;
        }

        // curl "localhost:5000/kubernetesmanifest/deployment?ns=default&name=kubernetesapisample"
        // curl "localhost:5000/kubernetesmanifest/deployment?ns=default&name=kubernetesapisample&watch=true"
        // response format: YAML
        [HttpGet("deployment")]
        public async Task<string> GetDeployment(string ns, string name, bool watch = false, string resourceVersion = "")
        {
            if (watch)
            {
                if (string.IsNullOrEmpty(resourceVersion))
                {
                    var deployments = await _operations.GetDeploymentsAsync(ns);
                    resourceVersion = deployments.metadata.resourceVersion;
                }
                _logger.LogInformation($"Watch deployment api. resourceVersion {resourceVersion}");
                var watchRes = await _operations.WatchDeploymentsManifestAsync(ns, resourceVersion);
                return watchRes;
            }
            else
            {
                _logger.LogInformation("Get deployment api.");
                var res = await _operations.GetDeploymentManifestAsync(ns, name);
                return res;
            }
        }
        #endregion

        #region jobs

        // curl localhost:5000/kubernetesmanifest/jobs
        // curl localhost:5000/kubernetesmanifest/jobs?ns=default
        // response format: YAML
        [HttpGet("jobs")]
        public async Task<string> GetJobs(string ns = "")
        {
            _logger.LogInformation("Get jobs manifest api.");
            var res = await _operations.GetJobsManifestAsync(ns);
            return res;
        }

        // curl "localhost:5000/kubernetesmanifest/job?ns=default&name=hoge"
        // curl "localhost:5000/kubernetesmanifest/job?ns=default&name=hoge&watch=true"
        // response format: YAML
        [HttpGet("job")]
        public async Task<string> GetJob(string ns, string name, bool watch = false, string resourceVersion = "")
        {
            if (watch)
            {
                if (string.IsNullOrEmpty(resourceVersion))
                {
                    var deployments = await _operations.GetJobsAsync(ns);
                    resourceVersion = deployments.metadata.resourceVersion;
                }
                _logger.LogInformation($"Watch job api. resourceVersion {resourceVersion}");
                var watchRes = await _operations.WatchJobsManifestAsync(ns, resourceVersion);
                return watchRes;
            }
            else
            {
                _logger.LogInformation("Get job api.");
                var res = await _operations.GetJobManifestAsync(ns, name);
                return res;
            }
        }
        #endregion
    }
}