using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Agones
{
    class MockAgonesSdk : IHostedService, IAgonesSdk
    {
        public Task<bool> Allocate()
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> GetGameServer()
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> Health()
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> Ready()
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> Reserve()
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> SetAnnotation(string key, string value)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> SetLabel(string key, string value)
        {
            return Task.FromResult<bool>(true);
        }

        public Task<bool> Shutdown()
        {
            return Task.FromResult<bool>(true);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<bool> WatchGameServer()
        {
            // stream どうするの?
            return Task.FromResult<bool>(true);
        }
    }
}
