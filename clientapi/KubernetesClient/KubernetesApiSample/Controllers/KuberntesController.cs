using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubernetesClient;
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

        [HttpGet("status")]
        public KubernetesClientStatus GetStatus()
        {
            logger.LogInformation("Get status.");
            var res = kubeapi.GetStatusAsync();
            return res;
        }

        [HttpGet("api")]
        public async Task<string> Get()
        {
            logger.LogInformation("Get api.");
            var res = await kubeapi.GetOpenApiSpecAsync();
            return res;
        }

        [HttpGet("configure_client")]
        [HttpPost("configure_client")]
        public KubernetesClientStatus ConfigureClient(bool skipCertificateValidate = false)
        {
            logger.LogInformation("Configure status.");
            kubeapi.ConfigureClient(skipCertificateValidate);
            var res = kubeapi.GetStatusAsync();
            return res;
        }
    }
}