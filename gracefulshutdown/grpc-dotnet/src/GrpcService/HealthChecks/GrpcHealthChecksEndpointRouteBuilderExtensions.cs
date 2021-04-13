using System;
using Grpc.HealthCheck;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    // borrow from: https://github.com/grpc/grpc-dotnet/tree/master/src/Grpc.AspNetCore.HealthChecks
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add gRPC service endpoints.
    /// </summary>
    public static class GrpcHealthChecksEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps incoming requests to the gRPC health checks service.
        /// This service can be queried to discover the health of the server.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <returns>An <see cref="GrpcServiceEndpointConventionBuilder"/> for endpoints associated with the service.</returns>
        public static GrpcServiceEndpointConventionBuilder MapGrpcHealthChecksService(this IEndpointRouteBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.MapGrpcService<HealthServiceImpl>();
        }
    }
}