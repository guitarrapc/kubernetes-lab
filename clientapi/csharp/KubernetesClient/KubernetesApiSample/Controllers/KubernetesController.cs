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

        #region pods

        // curl localhost:5000/kubernetes/pods
        // curl localhost:5000/kubernetes/pods?ns=default
        // curl "localhost:5000/kubernetes/pods?ns=default&watch=true"
        // curl "localhost:5000/kubernetes/pods?ns=default&labelSelector=app=hoge"
        // curl "localhost:5000/kubernetes/pods?ns=default&timeoutSeconds=0"
        // response format: JSON
        [HttpGet("pods")]
        public async Task<string[]> GetPods(string ns = "", bool watch = false, string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Get pods api");
            if (watch)
            {
                var deployments = await _operations.WatchPodsAsync(ns, "", labelSelector, timeoutSeconds);
                return new[] { $"{deployments.type} {deployments.@object.metadata.@namespace}/{deployments.@object.metadata.name}" };
            }
            else
            {
                var deployments = await _operations.GetPodsAsync(ns, labelSelector, timeoutSeconds);
                return deployments.items
                    .Select(item => $"{item.metadata.@namespace}/{item.metadata.name}")
                    .ToArray();
            }
        }

        // curl localhost:5000/kubernetes/pods/watch?ns=default
        // response format: JSON
        [HttpGet("pods/watch")]
        public async Task<V1WatchEvent<V1Pod>> WatchPods(string ns, string resourceVersion = "", string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Watch pods api. resourceVersion {resourceVersion}");
            var res = await _operations.WatchPodsAsync(ns, resourceVersion, labelSelector, timeoutSeconds);
            return res;
        }

        // curl localhost:5000/kubernetes/pods/watch2?ns=default
        // response format: JSON
        [HttpGet("pods/watch2")]
        public async Task<V1Pod[]> WatchPods2(string ns, string resourceVersion = "", TimeSpan? expire = default, string labelSelector = null, int? timeoutSeconds = null)
        {
            if (!expire.HasValue)
                expire = TimeSpan.FromSeconds(60);
            _logger.LogInformation($"Watch pods api. auto cancel after {expire}");

            var cts = new CancellationTokenSource(expire.Value);

            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetPodsAsync(ns, labelSelector, timeoutSeconds);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            int added = 0;
            var result = new List<V1Pod>();
            var res = _operations.GetPodsHttpAsync(ns, true, labelSelector, timeoutSeconds);
            using (var watch = res.Watch<V1Pod, V1PodList>((type, item, cts) =>
            {
                _logger.LogInformation($"{type}, {item.metadata.@namespace}/{item.metadata.name}");
                if (type == WatchEventType.Added)
                {
                    added++;
                    if (added >= 5)
                    {
                        cts.Cancel();
                    }
                    result.Add(item);
                }
            },
            ex =>
            {
                _logger.LogCritical(ex, ex.Message);
                cts.Cancel();
            }, () => _logger.LogInformation("watch closed."), cts))
            {
                await watch.Execute();
            }

            _logger.LogInformation($"return watch result, count {result.Count} names {string.Join(", ", result.Select(x => $"{x.metadata.@namespace}/{x.metadata.name}"))}");
            return result.ToArray();
        }

        // curl "localhost:5000/kubernetes/pod?ns=default&name=kubernetesapisample"
        // response format: JSON
        [HttpGet("pod")]
        public async Task<V1Pod> GetPod(string ns, string name, string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation("Get pod api.");
            var deployment = await _operations.GetPodAsync(ns, name, labelSelector, timeoutSeconds);
            return deployment;
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/pod -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogdjEKa2luZDogUG9kCm1ldGFkYXRhOgogIG5hbWU6IGZyb250ZW5kCnNwZWM6CiAgY29udGFpbmVyczoKICAgIC0gbmFtZTogcGhwLXJlZGlzCiAgICAgIGltYWdlOiBrOHMuZ2NyLmlvL2d1ZXN0Ym9vazp2MwogICAgICByZXNvdXJjZXM6CiAgICAgICAgcmVxdWVzdHM6CiAgICAgICAgICBjcHU6IDEwMG0KICAgICAgICAgIG1lbW9yeTogMTAwTWkKICAgICAgICBsaW1pdHM6CiAgICAgICAgICBjcHU6IDIwMDBtCiAgICAgICAgICBtZW1vcnk6IDEwMDBNaQogICAgICBlbnY6CiAgICAgICAgLSBuYW1lOiBHRVRfSE9TVFNfRlJPTQogICAgICAgICAgdmFsdWU6IGRucwogICAgICAgICAgIyBJZiB5b3VyIGNsdXN0ZXIgY29uZmlnIGRvZXMgbm90IGluY2x1ZGUgYSBkbnMgc2VydmljZSwgdGhlbiB0bwogICAgICAgICAgIyBpbnN0ZWFkIGFjY2VzcyBlbnZpcm9ubWVudCB2YXJpYWJsZXMgdG8gZmluZCBzZXJ2aWNlIGhvc3QKICAgICAgICAgICMgaW5mbywgY29tbWVudCBvdXQgdGhlICd2YWx1ZTogZG5zJyBsaW5lIGFib3ZlLCBhbmQgdW5jb21tZW50IHRoZQogICAgICAgICAgIyBsaW5lIGJlbG93OgogICAgICAgICAgIyB2YWx1ZTogZW52CiAgICAgIHBvcnRzOgogICAgICAgIC0gbmFtZTogaHR0cC1zZXJ2ZXIKICAgICAgICAgIGNvbnRhaW5lclBvcnQ6IDMwMDAK"}'
        // response format: JSON
        [HttpPost("pod")]
        public async Task<V1Pod> CreateOrUpdatePod(KubernetesCreateOrUpdateRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Create or Replace pod api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}");
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var deployment = await _operations.CreateOrReplacePodAsync(request.NameSpace, decodedBody, request.BodyContentType, labelSelector, timeoutSeconds);
            return deployment;
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/pod -d '{"namespace": "default", "name": "frontend"}'
        // response format: JSON
        [HttpDelete("pod")]
        public async Task<V1Pod> DeletePod(KubernetesDeleteRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Delete pod api. namespace {request.NameSpace}, name {request.Name}");
            var options = request.GraceperiodSecond.HasValue
                ? new V1DeleteOptions { gracePeriodSeconds = request.GraceperiodSecond.Value }
                : null;
            var status = await _operations.DeletePodAsync(request.NameSpace, request.Name, options, labelSelector, timeoutSeconds);
            return status;
        }
        #endregion

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
        public async Task<V1WatchEvent<V1Deployment>> WatchDeployments(string ns, string resourceVersion = "", string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Watch deployments api. resourceVersion {resourceVersion}");
            var res = await _operations.WatchDeploymentsAsync(ns, resourceVersion, labelSelector, timeoutSeconds);
            return res;
        }

        // curl localhost:5000/kubernetes/deployments/watch2?ns=default
        // response format: JSON
        [HttpGet("deployments/watch2")]
        public async Task<V1Deployment[]> WatchDeployments2(string ns, string resourceVersion = "", TimeSpan? expire = default, string labelSelector = null, int? timeoutSeconds = null)
        {
            if (!expire.HasValue)
                expire = TimeSpan.FromSeconds(60);
            _logger.LogInformation($"Watch deployments api. auto cancel after {expire}");

            var cts = new CancellationTokenSource(expire.Value);

            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetDeploymentsAsync(ns, labelSelector, timeoutSeconds);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            int added = 0;
            var result = new List<V1Deployment>();
            var res = _operations.GetDeploymentsHttpAsync(ns, true, labelSelector, timeoutSeconds);
            using (var watch = res.Watch<V1Deployment, V1DeploymentList>((type, item, cts) =>
            {
                _logger.LogInformation($"{type}, {item.metadata.@namespace}/{item.metadata.name}");
                if (type == WatchEventType.Added)
                {
                    added++;
                    if (added >= 5)
                    {
                        cts.Cancel();
                    }
                    result.Add(item);
                }
            },
            ex =>
            {
                _logger.LogCritical(ex, ex.Message);
                cts.Cancel();
            }, () => _logger.LogInformation("watch closed."), cts))
            {
                await watch.Execute();
            }

            _logger.LogInformation($"return watch result, count {result.Count} names {string.Join(", ", result.Select(x => $"{x.metadata.@namespace}/{x.metadata.name}"))}");
            return result.ToArray();
        }

        // curl "localhost:5000/kubernetes/deployment?ns=default&name=kubernetesapisample"
        // response format: JSON
        [HttpGet("deployment")]
        public async Task<V1Deployment> GetDeployment(string ns, string name, string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation("Get deployment api.");
            var deployment = await _operations.GetDeploymentAsync(ns, name, labelSelector, timeoutSeconds);
            return deployment;
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogYXBwcy92MSAjICBmb3IgazhzIHZlcnNpb25zIGJlZm9yZSAxLjkuMCB1c2UgYXBwcy92MWJldGEyICBhbmQgYmVmb3JlIDEuOC4wIHVzZSBleHRlbnNpb25zL3YxYmV0YTENCmtpbmQ6IERlcGxveW1lbnQNCm1ldGFkYXRhOg0KICBuYW1lOiBmcm9udGVuZA0Kc3BlYzoNCiAgc2VsZWN0b3I6DQogICAgbWF0Y2hMYWJlbHM6DQogICAgICBhcHA6IGd1ZXN0Ym9vaw0KICB0ZW1wbGF0ZToNCiAgICBtZXRhZGF0YToNCiAgICAgIGxhYmVsczoNCiAgICAgICAgYXBwOiBndWVzdGJvb2sNCiAgICBzcGVjOg0KICAgICAgY29udGFpbmVyczoNCiAgICAgICAgLSBuYW1lOiBwaHAtcmVkaXMNCiAgICAgICAgICBpbWFnZTogazhzLmdjci5pby9ndWVzdGJvb2s6djMNCiAgICAgICAgICByZXNvdXJjZXM6DQogICAgICAgICAgICByZXF1ZXN0czoNCiAgICAgICAgICAgICAgY3B1OiAxMDBtDQogICAgICAgICAgICAgIG1lbW9yeTogMTAwTWkNCiAgICAgICAgICAgIGxpbWl0czoNCiAgICAgICAgICAgICAgY3B1OiAyMDAwbQ0KICAgICAgICAgICAgICBtZW1vcnk6IDEwMDBNaQ0KICAgICAgICAgIGVudjoNCiAgICAgICAgICAgIC0gbmFtZTogR0VUX0hPU1RTX0ZST00NCiAgICAgICAgICAgICAgdmFsdWU6IGRucw0KICAgICAgICAgICAgICAjIElmIHlvdXIgY2x1c3RlciBjb25maWcgZG9lcyBub3QgaW5jbHVkZSBhIGRucyBzZXJ2aWNlLCB0aGVuIHRvDQogICAgICAgICAgICAgICMgaW5zdGVhZCBhY2Nlc3MgZW52aXJvbm1lbnQgdmFyaWFibGVzIHRvIGZpbmQgc2VydmljZSBob3N0DQogICAgICAgICAgICAgICMgaW5mbywgY29tbWVudCBvdXQgdGhlICd2YWx1ZTogZG5zJyBsaW5lIGFib3ZlLCBhbmQgdW5jb21tZW50IHRoZQ0KICAgICAgICAgICAgICAjIGxpbmUgYmVsb3c6DQogICAgICAgICAgICAgICMgdmFsdWU6IGVudg0KICAgICAgICAgIHBvcnRzOg0KICAgICAgICAgICAgLSBuYW1lOiBodHRwLXNlcnZlcg0KICAgICAgICAgICAgICBjb250YWluZXJQb3J0OiAzMDAwDQo="}'
        // response format: JSON
        [HttpPost("deployment")]
        public async Task<V1Deployment> CreateOrUpdateDeployment(KubernetesCreateOrUpdateRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Create or Replace deployment api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}");
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var deployment = await _operations.CreateOrReplaceDeploymentAsync(request.NameSpace, decodedBody, request.BodyContentType, labelSelector, timeoutSeconds);
            return deployment;
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/deployment -d '{"namespace": "default", "name": "frontend"}'
        // response format: JSON
        [HttpDelete("deployment")]
        public async Task<V1Status> DeleteDeployment(KubernetesDeleteRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Delete deployment api. namespace {request.NameSpace}, name {request.Name}");
            var options = request.GraceperiodSecond.HasValue
                ? new V1DeleteOptions { gracePeriodSeconds = request.GraceperiodSecond.Value }
                : null;
            var status = await _operations.DeleteDeploymentAsync(request.NameSpace, request.Name, options, labelSelector, timeoutSeconds);
            return status;
        }
        #endregion

        #region jobs

        // curl localhost:5000/kubernetes/jobs
        // curl localhost:5000/kubernetes/jobs?ns=default
        // response format: JSON
        [HttpGet("jobs")]
        public async Task<string[]> GetJobs(string ns = "", string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation("Get jobs api.");
            var jobs = await _operations.GetJobsAsync(ns, labelSelector, timeoutSeconds);
            return jobs.items
                .Select(item => $"{item.metadata.@namespace}/{item.metadata.name}")
                .ToArray();
        }

        // curl localhost:5000/kubernetes/jobs/watch?ns=default
        // response format: JSON
        [HttpGet("jobs/watch")]
        public async Task<V1WatchEvent<V1Job>> WatchJobs(string ns, string resourceVersion = "", string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Watch jobs api. resourceVersion {resourceVersion}");
            var res = await _operations.WatchJobsAsync(ns, resourceVersion, labelSelector, timeoutSeconds);
            return res;
        }

        // curl localhost:5000/kubernetes/jobs/watch2?ns=default
        // response format: JSON
        [HttpGet("jobs/watch2")]
        public async Task<V1Job[]> WatchJobs2(string ns, string resourceVersion = "", TimeSpan? expire = default, string labelSelector = null, int? timeoutSeconds = null)
        {
            if (!expire.HasValue)
                expire = TimeSpan.FromSeconds(60);
            _logger.LogInformation($"Watch jobs api. auto cancel after {expire}");

            var cts = new CancellationTokenSource(expire.Value);

            if (string.IsNullOrEmpty(resourceVersion))
            {
                var deployments = await _operations.GetJobsAsync(ns, labelSelector, timeoutSeconds);
                resourceVersion = deployments.metadata.resourceVersion;
            }
            int added = 0;
            var result = new List<V1Job>();
            var res = _operations.GetJobsHttpAsync(ns, true, labelSelector, timeoutSeconds);
            using (var watch = res.Watch<V1Job, V1JobList>((type, item, cts) =>
            {
                _logger.LogInformation($"{type}, {item.metadata.@namespace}/{item.metadata.name}");
                if (type == WatchEventType.Added)
                {
                    added++;
                    if (added >= 1)
                    {
                        cts.Cancel();
                    }
                    result.Add(item);
                }
            },
            ex =>
            {
                _logger.LogCritical(ex, ex.Message);
                cts.Cancel();
                throw ex;
            },
            () => _logger.LogInformation("watch closed."), cts))
            {
                await watch.Execute();
            }

            _logger.LogInformation($"return watch result, count {result.Count} names {string.Join(", ", result.Select(x => $"{x.metadata.@namespace}/{x.metadata.name}"))}");
            return result.ToArray();
        }

        // curl "localhost:5000/kubernetes/job?ns=default&name=hoge"
        // response format: JSON
        [HttpGet("job")]
        public async Task<V1Job> GetJob(string ns, string name, string labelSelector = null, int? timeoutSeconds = null)
        {
            _logger.LogInformation("Get job api.");
            var job = await _operations.GetJobAsync(ns, name, labelSelector, timeoutSeconds);
            return job;
        }

        // curl -X POST -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "body": "YXBpVmVyc2lvbjogYmF0Y2gvdjEKa2luZDogSm9iCm1ldGFkYXRhOgogIGxhYmVsczoKICAgIGFwcDogaG9nZQogIG5hbWU6IGhvZ2UKc3BlYzoKICBiYWNrb2ZmTGltaXQ6IDAKICBjb21wbGV0aW9uczogMwogIHBhcmFsbGVsaXNtOiAzCiAgdGVtcGxhdGU6CiAgICBtZXRhZGF0YToKICAgICAgbGFiZWxzOgogICAgICAgIGFwcDogaG9nZQogICAgc3BlYzoKICAgICAgY29udGFpbmVyczoKICAgICAgICAtIGltYWdlOiBob2dlCiAgICAgICAgICBpbWFnZVB1bGxQb2xpY3k6IEFsd2F5cwogICAgICAgICAgbmFtZTogaG9nZQogICAgICByZXN0YXJ0UG9saWN5OiBOZXZlcgo="}'
        // response format: JSON
        [HttpPost("job")]
        public async Task<V1Job> CreateOrUpdateJob(KubernetesCreateOrUpdateRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Create or Replace job api. namespace {request.NameSpace}, bodyContentType {request.BodyContentType}");
            var decodedBody = Kubernetes.Base64ToString(request.Body);
            var job = await _operations.CreateOrReplaceJobAsync(request.NameSpace, decodedBody, request.BodyContentType, labelSelector, timeoutSeconds);
            return job;
        }

        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "name": "hoge"}'
        // curl -X DELETE -H "Content-Type: application/json" localhost:5000/kubernetes/job -d '{"namespace": "default", "name": "hoge", "gracePeriodSeconds": 0}'
        // response format: JSON
        [HttpDelete("job")]
        public async Task<V1Job> DeleteJob(KubernetesDeleteRequest request, [FromQuery]string labelSelector = null, [FromQuery]int? timeoutSeconds = null)
        {
            _logger.LogInformation($"Delete job api. namespace {request.NameSpace}, name {request.Name}");
            var options = new V1DeleteOptions
            {
                propagationPolicy = "Foreground",
                gracePeriodSeconds = request.GraceperiodSecond,
            };
            var res = await _operations.DeleteJobAsync(request.NameSpace, request.Name, options, labelSelector, timeoutSeconds);
            return res;
        }

        #endregion
    }
}