using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService
{
    public class ConnectorService : Connector.ConnectorBase
    {
        private readonly ILogger<ConnectorService> _logger;
        private readonly IHostApplicationLifetime _lifeTime;
        private readonly ConnectionStateProvider _connectionStateProvider;

        public ConnectorService(ConnectionStateProvider connectionStateProvider, ILogger<ConnectorService> logger, IHostApplicationLifetime lifeTime)
        {
            _connectionStateProvider = connectionStateProvider;
            _logger = logger;
            _lifeTime = lifeTime;
        }

        public override async Task Subscribe(IAsyncStreamReader<ActionMessage> requestStream, IServerStreamWriter<UpdateMessage> responseStream, ServerCallContext context)
        {
            _connectionStateProvider.Connect();
            context.CancellationToken.Register(() => _connectionStateProvider.Disconnect());
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken, _lifeTime.ApplicationStopping);

            var readTask = Task.Run(async () =>
            {
                _logger.LogInformation($"Request Header:");
                foreach (var header in context.RequestHeaders)
                {
                    _logger.LogInformation($"  {header.Key} = {header.Value}");
                }

                try
                {
                    await foreach (var message in requestStream.ReadAllAsync(context.CancellationToken))
                    {
                        await responseStream.WriteAsync(new UpdateMessage()
                        {
                            Message = "Echo Duplex " + message.Name,
                        });
                        _logger.LogInformation(message.Name);
                    }
                }
                catch (TaskCanceledException)
                {
                    // client disconnected while command executing
                }
                catch (System.IO.IOException)
                {
                    // client disconnected
                }

            }, context.CancellationToken);

            try
            {
                while (!linkedCts.Token.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(new UpdateMessage()
                    {
                        Message = "Duplex",
                    });
                    await Task.Delay(TimeSpan.FromMilliseconds(500), linkedCts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                // client disconnected while command executing
            }

            // await readTask;
        }
    }
}
