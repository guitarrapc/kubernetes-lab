using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;

namespace KubernetesClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync<KubernetesApp>(args);
        }
    }

    public class KubernetesApp : ConsoleAppBase
    {
        private KubernetesApi api;
        public KubernetesApp(KubernetesApi api)
        {
            this.api = new KubernetesApi();
        }

        public async ValueTask GetOpenApiSpec()
        {
            if (!api.IsRunningOnKubernetes)
            {
                Console.WriteLine("App not run on Kubernetes. Quit command.");
                return;
            }

            var res = await api.GetOpenApiSpecAsync();
            Console.WriteLine(res);
        }
    }

    public class KubernetesClientStatus
    {
        public string AccessToken { get; set; }

        public string HostName { get; set; }

        public bool IsRunningOnKubernetes { get; set; }

        public string KubernetesServiceEndPoint { get; set; }

        public string Namespace { get; set; }

        public bool SkipCertificationValidation { get; set; }
    }
    
    public class KubernetesApi
    {
        public bool IsRunningOnKubernetes { get; }
        private IKubernetesClient _provider;

        public KubernetesApi()
        {
            _provider = GetDefaultProvider();
            IsRunningOnKubernetes = _provider.IsRunningOnKubernetes;
        }

        public KubernetesClientStatus GetStatusAsync()
        {
            var status = new KubernetesClientStatus
            {
                AccessToken = _provider.AccessToken,
                HostName = _provider.HostName,
                IsRunningOnKubernetes = _provider.IsRunningOnKubernetes,
                KubernetesServiceEndPoint = _provider.KubernetesServiceEndPoint,
                Namespace = _provider.Namespace,
                SkipCertificationValidation = _provider.SkipCertificationValidation,
            };
            return status;
        }

        public void ConfigureClient(bool skipCertficateValidate)
        {
            _provider.SkipCertificationValidation = skipCertficateValidate;
        }

        public async ValueTask<string> GetApiAsync(string apiPath)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath);
                return res;
            }
        }

        /// <summary>
        /// OpenAPI Swagger Definition. https://kubernetes.io/ja/docs/concepts/overview/kubernetes-api/
        /// </summary>
        /// <returns></returns>
        public async ValueTask<string> GetOpenApiSpecAsync()
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                var apiPath = "/openapi/v2";
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath);
                return res;
            }
        }

        private static IKubernetesClient GetDefaultProvider()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new NotImplementedException($"{Environment.OSVersion.Platform} is not supported.");
            return (IKubernetesClient)new UnixKubernetesClient();
        }
    }

    public interface IKubernetesClient
    {
        string AccessToken { get; }
        string HostName { get; }
        bool IsRunningOnKubernetes { get; }
        string KubernetesServiceEndPoint { get; }
        string Namespace { get; }
        bool SkipCertificationValidation { get; set; }

        HttpClient CreateHttpClient();
    }

    public class UnixKubernetesClient : KubernetesClientBase
    {
        private bool? _isRunningOnKubernetes;
        private string _namespace;
        private string _hostName;
        private string _accessToken;
        private string _kubernetesServiceEndPoint;

        public override bool IsRunningOnKubernetes
            => _isRunningOnKubernetes ?? (bool)(_isRunningOnKubernetes = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")));

        public override string AccessToken
            => _accessToken ?? (_accessToken = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token"));

        public override string HostName
            => _hostName ?? (_hostName = Environment.GetEnvironmentVariable("HOSTNAME"));

        public override string KubernetesServiceEndPoint
            => _kubernetesServiceEndPoint ?? (_kubernetesServiceEndPoint = $"https://{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT")}");

        public override string Namespace
            => _namespace ?? (_namespace = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/namespace"));
    }

    public abstract class KubernetesClientBase : IKubernetesClient
    {
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string Namespace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificationValidation { get; set; }

        public HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            if (SkipCertificationValidation)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = /* HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; */ delegate { return true; };
            }

            return httpClient;
        }
    }
}
