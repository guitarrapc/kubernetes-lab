using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService
{
    public static class ExtendHostLifeExtensions
    {
        public static void ExtendHostLife(this IServiceCollection services)
        {
            services.Configure<HostOptions>(options =>
            {
                // extend life when deploy executed
                options.ShutdownTimeout = TimeSpan.FromMinutes(31);
            });

            services.AddSingleton<ConnectionStateProvider>();

            services.AddHostedService<ExtendLifeService>();
        }
    }

    public class ConnectionStateProvider
    {
        public int Connections => _referenceCount;
        public bool Alive => _referenceCount > 0;

        private int _referenceCount = 0;

        public void Connect()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public void Disconnect()
        {
            Interlocked.Decrement(ref _referenceCount);
        }
    }

    public class ExtendLifeService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly ConnectionStateProvider _connectionState;

        public ExtendLifeService(ConnectionStateProvider connectionState, ILogger<ExtendLifeService> logger)
        {
            _connectionState = connectionState;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ExtendLifeService)} is started.");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(ExtendLifeService)} is shutting down. Waiting for connection disconnect.");
            await MonitorDisconnectAsync();
            _logger.LogInformation($"{_connectionState.Connections} are remained.");

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        private async Task MonitorDisconnectAsync()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(30));
            while (!cts.IsCancellationRequested)
            {
                var remainConnections = _connectionState.Connections;
                if (remainConnections == 0)
                {
                    _logger.LogInformation($"All connections completed.");
                    break;
                }

                _logger.LogInformation($"Waiting complete connections. Connections {remainConnections}");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
