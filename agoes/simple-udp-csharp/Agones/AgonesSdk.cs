using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Agones
{
    // ref: sdk sample https://github.com/googleforgames/agones/blob/release-1.0.0/sdks/go/sdk.go
    public class AgonesSdk : IHostedService, IAgonesSdk
    {
        public double HealthIntervalSecond { get; set; } = 5.0;
        public bool HealthEnabled { get; set; } = true;
        public double WatchIntervalSecond { get; set; } = 5.0;
        public bool WatchGameServerEnabled { get; set; } = true;

        // ref: sdk server https://github.com/googleforgames/agones/blob/master/cmd/sdk-server/main.go
        // grpc: localhost on port 59357
        // http: localhost on port 59358
        readonly Uri SideCarAddress = new Uri("http://localhost:59358");
        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<IAgonesSdk> _logger;

        public AgonesSdk(IHttpClientFactory httpClientFactory, ILogger<IAgonesSdk> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // entrypoint for IHostedService
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HealthCheckAsync(cancellationTokenSource);
        }

        // exit for IHostedService
        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Dispose();
            return Task.CompletedTask;
        }

        public async Task<bool> Ready()
        {
            return await SendRequestAsync("/ready", "{}");
        }

        public async Task<bool> Allocate()
        {
            return await SendRequestAsync("/allocate", "{}");
        }

        public async Task<bool> Shutdown()
        {
            return await SendRequestAsync("/shutdown", "{}");
        }

        public async Task<bool> Health()
        {
            return await SendRequestAsync("/health", "{}");
        }

        public async Task<bool> GetGameServer()
        {
            // TODO: return GameServer
            return await SendRequestAsync("/getgameserver", "{}");
        }

        public async Task<bool> WatchGameServer()
        {
            return await SendRequestAsync("/watchgameserver", "{}");
        }

        public async Task<bool> Reserve()
        {
            return await SendRequestAsync("/reserve", "{}");
        }

        public async Task<bool> SetLabel(string key, string value)
        {
            string json = Utf8Json.JsonSerializer.ToJsonString(new KeyValueMessage(key, value));
            return await SendRequestAsync("/metadata/label", json, HttpMethod.Put);
        }

        public async Task<bool> SetAnnotation(string key, string value)
        {
            string json = Utf8Json.JsonSerializer.ToJsonString(new KeyValueMessage(key, value));
            return await SendRequestAsync("/metadata/annotation", json, HttpMethod.Put);
        }

        private async Task HealthCheckAsync(CancellationTokenSource cts)
        {
            while (HealthEnabled)
            {
                if (cts.IsCancellationRequested) throw new OperationCanceledException();

                await Task.Delay(TimeSpan.FromSeconds(HealthIntervalSecond));

                try
                {
                    await Health();
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        }

        private async Task WatchGameServerAsync(CancellationTokenSource cts)
        {
            while (WatchGameServerEnabled)
            {
                if (cts.IsCancellationRequested) throw new OperationCanceledException();

                await Task.Delay(TimeSpan.FromSeconds(WatchIntervalSecond));

                try
                {
                    await WatchGameServer();
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        }
        private async Task<bool> SendRequestAsync(string api, string json)
        {
            return await SendRequestAsync(api, json, HttpMethod.Post);
        }

        private async Task<bool> SendRequestAsync(string api, string json, HttpMethod method)
        {
            if (cancellationTokenSource.IsCancellationRequested) return false;

            var httpClient = _httpClientFactory.CreateClient("agones");
            httpClient.BaseAddress = SideCarAddress;
            var requestMessage = new HttpRequestMessage(method, api);
            var request = await httpClient.SendAsync(requestMessage);
            var content = await request.Content.ReadAsStringAsync();

            var ok = request.StatusCode == HttpStatusCode.OK;
            if (ok)
            {
                _logger.LogDebug($"Agones SendRequest ok: {api} {content}");
            }
            else
            {
                _logger.LogDebug($"Agones SendRequest failed: {api} {request.ReasonPhrase}");
            }

            return ok;
        }

        public class KeyValueMessage
        {
            public string Key;
            public string Value;
            public KeyValueMessage(string key, string value) => (Key, Value) = (key, value);
        }
    }
}
