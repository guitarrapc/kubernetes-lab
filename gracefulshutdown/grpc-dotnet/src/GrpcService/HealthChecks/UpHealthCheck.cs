using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService.HealthChecks
{
    public class UpHealthcheck : IHealthCheck
    {
        private readonly HealthCheckResult _result;

        public UpHealthcheck()
        {
            _result = new HealthCheckResult(HealthStatus.Healthy);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_result);
        }
    }
}
