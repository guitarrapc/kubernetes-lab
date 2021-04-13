using System;
using Grpc.AspNetCore.HealthChecks;
using Grpc.HealthCheck;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using GrpcService.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    // borrow from: https://github.com/grpc/grpc-dotnet/tree/master/src/Grpc.AspNetCore.HealthChecks
    /// <summary>
    /// Extension methods for the gRPC health checks services.
    /// </summary>
    public static class GrpcHealthChecksServiceExtensions
    {
        /// <summary>
        /// Adds gRPC health check services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <returns>An instance of <see cref="IHealthChecksBuilder"/> from which health checks can be registered.</returns>
        public static IHealthChecksBuilder AddGrpcHealthChecks(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // HealthServiceImpl is designed to be a singleton
            services.TryAddSingleton<HealthServiceImpl>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHealthCheckPublisher, GrpcHealthChecksPublisher>());

            // grpc_health_probe のdefault health = Service名が"" への応答
            return services.AddHealthChecks()
                .AddCheck<UpHealthcheck>("");
        }
    }
}