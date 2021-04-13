using Grpc.Core;
using Grpc.Health.V1;
using Grpc.Net.Client;
using GrpcService;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            var r1 = new Client("http://localhost:5000", "r1");
            var r2 = new Client("http://localhost:5000", "r2");
            //var r1 = new Client("https://test-app.eks-sandbox.dev.cysharp.io:443", "r1");
            //var r2 = new Client("https://test-app.eks-sandbox.dev.cysharp.io:443", "r2");
            r1.HealthCheck();
            await Task.WhenAll(r1.RunUnaryAsync(), r2.RunUnaryAsync());
            await Task.WhenAll(r1.RunDuplexAsync(), r2.RunDuplexAsync());

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public class Client : IAsyncDisposable
    {
        private readonly string _prefix;
        private readonly GrpcChannel _channel;

        private Task _readTask;
        private AsyncDuplexStreamingCall<ActionMessage, UpdateMessage> _duplexStreaming;

        public Client(string endpoint, string prefix)
        {
            _prefix = prefix;

            // The port number(5001) must match the port of the gRPC server.
            var handler = new SocketsHttpHandler();
            if (endpoint.StartsWith("https://"))
            {
                handler.SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true,
                };
            }
            _channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions
            {
                HttpHandler = handler,                
            });
        }

        public void HealthCheck()
        {
            var client = new Health.HealthClient(_channel);
            var response = client.Check(new HealthCheckRequest
            {
                Service = ""
            });
            Console.WriteLine(response.Status);
        }
        public async Task RunUnaryAsync()
        {
            // unary
            var client = new Greeter.GreeterClient(_channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = $"{_prefix} GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
        }

        public async Task RunDuplexAsync()
        {
            // duplex
            var requestHeaders = new Metadata
            {
                { "x-host-port", "10-0-0-10" },
            };
            var duplexClient = new Connector.ConnectorClient(_channel);
            _duplexStreaming = duplexClient.Subscribe(requestHeaders);
            _readTask = Task.Run(async () =>
            {
                await foreach (var response in _duplexStreaming.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine(response.Message);
                }
            });

            var i = 0;
            try
            {
                while (i++ < 100)
                {
                    await _duplexStreaming.RequestStream.WriteAsync(new ActionMessage
                    {
                        Name = $"{_prefix} {i}",
                    });
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                }
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.OK)
            {
                return;
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Finished.");
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await _duplexStreaming.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _readTask.ConfigureAwait(false); ;
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                _duplexStreaming.Dispose();
            }
        }
    }
}
