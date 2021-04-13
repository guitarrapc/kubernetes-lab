using System.Threading;
using System.Threading.Tasks;
using Grpc.Health.V1;
using Grpc.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Grpc.AspNetCore.HealthChecks
{
    // borrow from: https://github.com/grpc/grpc-dotnet/tree/master/src/Grpc.AspNetCore.HealthChecks
    internal class GrpcHealthChecksPublisher : IHealthCheckPublisher
    {
        private readonly HealthServiceImpl _healthService;

        public GrpcHealthChecksPublisher(HealthServiceImpl healthService)
        {
            _healthService = healthService;
        }

        /// <summary>
        /// Call every 30sec in default
        /// </summary>
        /// <param name="report"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            // note: healthcheck が一つは必要
            foreach (var entry in report.Entries)
            {
                var status = entry.Value.Status;

                _healthService.SetStatus(entry.Key, ResolveStatus(status));
            }

            return Task.CompletedTask;
        }

        private static HealthCheckResponse.Types.ServingStatus ResolveStatus(HealthStatus status)
        {
            return status == HealthStatus.Unhealthy
                ? HealthCheckResponse.Types.ServingStatus.NotServing
                : HealthCheckResponse.Types.ServingStatus.Serving;
        }
    }

}