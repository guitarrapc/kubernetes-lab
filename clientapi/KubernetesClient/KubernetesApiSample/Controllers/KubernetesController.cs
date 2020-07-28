using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ILogger<KubernetesController> _logger;
        private readonly Kubernetes _operations;

        public KubernetesController(Kubernetes operations, ILogger<KubernetesController> logger)
        {
            _operations = operations;
            _logger = logger;
        }

        // curl localhost:5000/kubernetes/status
        // response format: always json response.
        [HttpGet("status")]
        public KubernetesClientStatusResponse GetStatus()
        {
            _logger.LogInformation("Get status.");
            var res = _operations.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/configure_client?skipcertificatevalidate=true
        // response format: JSON
        [HttpGet("configure_client")]
        [HttpPost("configure_client")]
        public KubernetesClientStatusResponse ConfigureClient(bool skipCertificateValidate = true)
        {
            _logger.LogInformation("Configure status.");
            _operations.ConfigureClient(skipCertificateValidate);
            var res = _operations.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/configure_response?isJson=true
        // response format: JSON
        [HttpGet("configure_response")]
        [HttpPost("configure_response")]
        public KubernetesClientStatusResponse ConfigureResponse(bool isJson = false)
        {
            _logger.LogInformation("Configure response.");
            _operations.ConfigureResponse(isJson);
            var res = _operations.GetStatus();
            return res;
        }

        // curl localhost:5000/kubernetes/spec
        // response format: JSON
        [HttpGet("spec")]
        public async Task<string> GetSpec()
        {
            _logger.LogInformation("Get spec api.");
            var res = await _operations.GetOpenApiSpecAsync();
            return res;
        }

        #region deployments

        // curl localhost:5000/kubernetes/deployments
        // curl localhost:5000/kubernetes/deployments?ns=default
        // response format: JSON
        [HttpGet("deployments")]
        public async Task<string[]> GetDeployments(string ns = "")
        {
            _logger.LogInformation("Get deployments api.");
            var deployments = await _operations.GetDeploymentsAsync(ns);
            return deployments.items
                .Select(item => $"{item.metadata.@namespace}/{item.metadata.name}")
                .ToArray();
        }

        // curl localhost:5000/kubernetes/deployments/watch?ns=default
        // response format: JSON
        [HttpGet("deployments/watch")]
        public async Task<V1WatchEvent> WatchDeployments(string ns, string resourceVersion = "")
        {
            _logger.LogInformation($"Watch deployments api. resourceVersion {resourceVersion}");
            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetDeploymentsAsync(ns);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            var res = await _operations.WatchDeploymentsAsync(ns, resourceVersion);
            return res;
        }

        // curl localhost:5000/kubernetes/deployments/watch2?ns=default
        // response format: JSON
        [HttpGet("deployments/watch2")]
        public async Task<V1Deployment[]> WatchDeployments2(string ns, string resourceVersion = "", TimeSpan? expire = default)
        {
            if (!expire.HasValue)
                expire = TimeSpan.FromSeconds(60);
            _logger.LogInformation($"Watch deployments api. auto cancel after {expire}");

            var cts = new CancellationTokenSource(expire.Value);

            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetDeploymentsAsync(ns);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            int added = 0;
            var result = new List<V1Deployment>();
            //var res = _operations.WatchDeploymentsHttpAsync(ns, resourceVersion);
            //using (var watch = res.Watch<V1Deployment, V1WatchEvent>((type, item, cts) =>
            var res = _operations.GetDeploymentsHttpAsync(ns, true);
            using (var watch = res.Watch<V1Deployment, V1DeploymentList>((type, item, cts) =>
            {
                _logger.LogInformation("Watch event");
                _logger.LogInformation(type.ToString());
                _logger.LogInformation(item.ToString());
                if (type == WatchEventType.Added)
                {
                    added++;
                    if (added >= 5)
                    {
                        cts.Cancel();
                    }
                    result.Add(item);
                }
            }, ex => _logger.LogCritical(ex, ex.Message), () => _logger.LogInformation("watch closed."), cts))
            {
                await watch.WatchLoop();
            }

            _logger.LogInformation($"return watch result, count {result.Count} names {string.Join(", ", result.Select(x => $"{x.metadata.@namespace}/{x.metadata.name}"))}");
            return result.ToArray();
        }

        // curl "localhost:5000/kubernetes/deployment?ns=default&name=kubernetesapisample"
        // response format: JSON
        [HttpGet("deployment")]
        public async Task<V1Deployment> GetDeployment(string ns, string name)
        {
            _logger.LogInformation("Get deployment api.");
            var deployment = await _operations.GetDeploymentAsync(ns, name);
            return deployment;
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogYXBwcy92MSAjICBmb3IgazhzIHZlcnNpb25zIGJlZm9yZSAxLjkuMCB1c2UgYXBwcy92MWJldGEyICBhbmQgYmVmb3JlIDEuOC4wIHVzZSBleHRlbnNpb25zL3YxYmV0YTENCmtpbmQ6IERlcGxveW1lbnQNCm1ldGFkYXRhOg0KICBuYW1lOiBmcm9udGVuZA0Kc3BlYzoNCiAgc2VsZWN0b3I6DQogICAgbWF0Y2hMYWJlbHM6DQogICAgICBhcHA6IGd1ZXN0Ym9vaw0KICB0ZW1wbGF0ZToNCiAgICBtZXRhZGF0YToNCiAgICAgIGxhYmVsczoNCiAgICAgICAgYXBwOiBndWVzdGJvb2sNCiAgICBzcGVjOg0KICAgICAgY29udGFpbmVyczoNCiAgICAgICAgLSBuYW1lOiBwaHAtcmVkaXMNCiAgICAgICAgICBpbWFnZTogazhzLmdjci5pby9ndWVzdGJvb2s6djMNCiAgICAgICAgICByZXNvdXJjZXM6DQogICAgICAgICAgICByZXF1ZXN0czoNCiAgICAgICAgICAgICAgY3B1OiAxMDBtDQogICAgICAgICAgICAgIG1lbW9yeTogMTAwTWkNCiAgICAgICAgICAgIGxpbWl0czoNCiAgICAgICAgICAgICAgY3B1OiAyMDAwbQ0KICAgICAgICAgICAgICBtZW1vcnk6IDEwMDBNaQ0KICAgICAgICAgIGVudjoNCiAgICAgICAgICAgIC0gbmFtZTogR0VUX0hPU1RTX0ZST00NCiAgICAgICAgICAgICAgdmFsdWU6IGRucw0KICAgICAgICAgICAgICAjIElmIHlvdXIgY2x1c3RlciBjb25maWcgZG9lcyBub3QgaW5jbHVkZSBhIGRucyBzZXJ2aWNlLCB0aGVuIHRvDQogICAgICAgICAgICAgICMgaW5zdGVhZCBhY2Nlc3MgZW52aXJvbm1lbnQgdmFyaWFibGVzIHRvIGZpbmQgc2VydmljZSBob3N0DQogICAgICAgICAgICAgICMgaW5mbywgY29tbWVudCBvdXQgdGhlICd2YWx1ZTogZG5zJyBsaW5lIGFib3ZlLCBhbmQgdW5jb21tZW50IHRoZQ0KICAgICAgICAgICAgICAjIGxpbmUgYmVsb3c6DQogICAgICAgICAgICAgICMgdmFsdWU6IGVudg0KICAgICAgICAgIHBvcnRzOg0KICAgICAgICAgICAgLSBuYW1lOiBodHRwLXNlcnZlcg0KICAgICAgICAgICAgICBjb250YWluZXJQb3J0OiAzMDAwDQo="}'
        // response format: JSON
        [HttpPost("deployment")]
        public async Task<V1Deployment> CreateOrUpdateDeployment(KubernetesCreateOrUpdateRequest request)
        {
            _logger.LogInformation($"Create or Replace deployment api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}, body {request.Body}");
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var deployment = await _operations.CreateOrReplaceDeploymentAsync(request.NameSpace, decodedBody, request.BodyContentType);
            return deployment;
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "name": "frontend"}'
        // response format: JSON
        [HttpDelete("deployment")]
        public async Task<V1Status> DeleteDeployment(KubernetesDeleteRequest request)            
        {
            _logger.LogInformation($"Delete deployment api. namespace {request.NameSpace}, name {request.Name}");
            var options = request.GraceperiodSecond.HasValue
                ? new V1DeleteOptions { gracePeriodSeconds = request.GraceperiodSecond.Value }
                : null;
            var status = await _operations.DeleteDeploymentAsync(request.NameSpace, request.Name, options);
            return status;
        }
        #endregion

        #region jobs

        // curl localhost:5000/kubernetes/jobs
        // curl localhost:5000/kubernetes/jobs?ns=default
        // response format: JSON
        [HttpGet("jobs")]
        public async Task<string[]> GetJobs(string ns = "")
        {
            _logger.LogInformation("Get jobs api.");
            var jobs = await _operations.GetJobsAsync(ns);
            return jobs.items
                .Select(item => $"{item.metadata.@namespace}/{item.metadata.name}")
                .ToArray();
        }

        // curl localhost:5000/kubernetes/jobs/watch?ns=default
        // response format: JSON
        [HttpGet("jobs/watch")]
        public async Task<V1WatchEvent> WatchJobs(string ns, string resourceVersion = "")
        {
            _logger.LogInformation($"Watch jobs api. resourceVersion {resourceVersion}");
            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetDeploymentsAsync(ns);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            var res = await _operations.WatchJobsAsync(ns, resourceVersion);
            return res;
        }

        // curl "localhost:5000/kubernetes/job?ns=default&name=hoge"
        // response format: JSON
        [HttpGet("job")]
        public async Task<V1Job> GetJob(string ns, string name)
        {
            _logger.LogInformation("Get job api.");
            var job = await _operations.GetJobAsync(ns, name);
            return job;
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogYmF0Y2gvdjEKa2luZDogSm9iCm1ldGFkYXRhOgogIGxhYmVsczoKICAgIGFwcDogaG9nZQogIG5hbWU6IGhvZ2UKc3BlYzoKICBiYWNrb2ZmTGltaXQ6IDYKICBjb21wbGV0aW9uczogMQogIHBhcmFsbGVsaXNtOiAxCiAgdGVtcGxhdGU6CiAgICBtZXRhZGF0YToKICAgICAgbGFiZWxzOgogICAgICAgIGFwcDogaG9nZQogICAgc3BlYzoKICAgICAgY29udGFpbmVyczoKICAgICAgICAtIGltYWdlOiBob2dlCiAgICAgICAgICBpbWFnZVB1bGxQb2xpY3k6IEFsd2F5cwogICAgICAgICAgbmFtZTogaG9nZQogICAgICByZXN0YXJ0UG9saWN5OiBOZXZlcgo="}'
        // response format: JSON
        [HttpPost("job")]
        public async Task<V1Job> CreateOrUpdateJob(KubernetesCreateOrUpdateRequest request)
        {
            _logger.LogInformation($"Create or Replace job api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}, body {request.Body}");
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var job = await _operations.CreateOrReplaceJobAsync(request.NameSpace, decodedBody, request.BodyContentType);
            return job;
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "name": "hoge"}'
        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "name": "hoge", "gracePeriodSeconds": 0}'
        // response format: JSON
        [HttpDelete("job")]
        public async Task<V1Job> DeleteJob(KubernetesDeleteRequest request)
        {
            _logger.LogInformation($"Delete job api. namespace {request.NameSpace}, name {request.Name}");
            var options = new V1DeleteOptions
            {
                propagationPolicy = "Foreground",
                gracePeriodSeconds = request.GraceperiodSecond,
            };
            var res = await _operations.DeleteJobAsync(request.NameSpace, request.Name, options);
            return res;
        }

        #endregion
    }
}