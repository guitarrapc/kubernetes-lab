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
    [Route("[controller]")]
    [ApiController]
    public class KuberntesController : ControllerBase
    {
        private ILogger<KuberntesController> logger;
        private KubernetesApi kubeapi;

        public KuberntesController(KubernetesApi kubeapi, ILogger<KuberntesController> logger)
        {
            this.kubeapi = kubeapi;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            logger.LogInformation("Get request.");
            var res = await kubeapi.GetOpenApiSpecAsync();
            return res;
        }
    }
}