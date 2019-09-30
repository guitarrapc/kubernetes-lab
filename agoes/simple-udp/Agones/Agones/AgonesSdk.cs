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
    public class AgonesSdk : IHostedService, IAgonesSdk
    {
        public double HealthIntervalSecond { get; private set; } = 5.0;
        public bool HealthEnabled { get; private set; } = true;
        public bool logEnabled { get; private set; } = false;

        // https://github.com/googleforgames/agones/blob/release-1.0.0/sdks/go/sdk.go
        // localhost on port 59357
        readonly Uri SideCarAddress = new Uri("http://localhost:59357");
        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<IAgonesSdk> _logger;

        public AgonesSdk(IHttpClientFactory httpClientFactory, ILogger<IAgonesSdk> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            await HealthCheckAsync(cts);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource?.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Marks this Game Server as ready to receive connections.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation and returns true if the request was successful.
        /// </returns>
        public async Task<bool> Ready()
        {
            return await SendRequestAsync("/ready", "{}");
        }

        /// <summary>
        /// Marks this Game Server as ready to shutdown.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation and returns true if the request was successful.
        /// </returns>
        public async Task<bool> Shutdown()
        {
            return await SendRequestAsync("/shutdown", "{}");
        }

        /// <summary>
        /// Marks this Game Server as Allocated.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation and returns true if the request was successful.
        /// </returns>
        public async Task<bool> Allocate()
        {
            return await SendRequestAsync("/allocate", "{}");
        }

        /// <summary>
        /// Set a metadata label that is stored in k8s.
        /// </summary>
        /// <param name="key">label key</param>
        /// <param name="value">label value</param>
        /// <returns>
        /// A task that represents the asynchronous operation and returns true if the request was successful.
        /// </returns>
        public async Task<bool> SetLabel(string key, string value)
        {
            string json = Utf8Json.JsonSerializer.ToJsonString(new KeyValueMessage(key, value));
            return await SendRequestAsync("/metadata/label", json, HttpMethod.Put);
        }

        /// <summary>
        /// Set a metadata annotation that is stored in k8s.
        /// </summary>
        /// <param name="key">annotation key</param>
        /// <param name="value">annotation value</param>
        /// <returns>
        /// A task that represents the asynchronous operation and returns true if the request was successful.
        /// </returns>
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
                    await SendRequestAsync("/health", "{}");
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
                _logger.LogInformation($"Agones SendRequest ok: {api} {content}");
            }
            else
            {
                _logger.LogInformation($"Agones SendRequest failed: {api} {request.ReasonPhrase}");
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
