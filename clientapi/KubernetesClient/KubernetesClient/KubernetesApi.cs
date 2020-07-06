using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KubernetesClient.Responses;

namespace KubernetesClient
{
    public class KubernetesApiConfig
    {
        public bool SkipCertificateValidation { get; set; }
        public ResponseType ResponseType { get; set; } = ResponseType.Json;

        public void Configure(IKubernetesClient _provider)
        {
            _provider.SkipCertificationValidation = SkipCertificateValidation;
        }
    }

    public class KubernetesApi
    {
        public bool IsRunningOnKubernetes { get; }
        private IKubernetesClient _provider;
        private KubernetesApiConfig _config = new KubernetesApiConfig();

        public KubernetesApi()
        {
            _provider = GetDefaultProvider();
            SetProviderConfig();
            IsRunningOnKubernetes = _provider.IsRunningOnKubernetes;
        }
        public KubernetesApi(KubernetesApiConfig config)
        {
            _config = config;
            _provider = GetDefaultProvider();
            SetProviderConfig();

            IsRunningOnKubernetes = _provider.IsRunningOnKubernetes;
        }

        #region API
        public KubernetesClientStatusResponse GetStatusAsync()
        {
            var status = new KubernetesClientStatusResponse
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
            _config.SkipCertificateValidation = skipCertficateValidate;
            SetProviderConfig();
        }

        public async ValueTask<string> GetApiAsync(string apiPath)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient);
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath);
                return res;
            }
        }

        public async ValueTask<string> PostApiAsync(string apiPath, string body, string bodyContenType = "application/json",  CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient);
                var content = new StringContent(body, Encoding.UTF8, bodyContenType);
                var res = await httpClient.PostAsync(_provider.KubernetesServiceEndPoint + apiPath, content, ct);
                res.EnsureSuccessStatusCode();
                var responseContent = await content.ReadAsStringAsync();
                return responseContent;
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
                // must be json. do not set ResponseType
                var apiPath = "/openapi/v2";
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath);
                return res;
            }
        }
        #endregion

        private static IKubernetesClient GetDefaultProvider()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                throw new NotImplementedException($"{Environment.OSVersion.Platform} is not supported.");
            return (IKubernetesClient)new UnixKubernetesClient();
        }
        private void SetProviderConfig()
        {
            _provider.SkipCertificationValidation = _config.SkipCertificateValidation;
        }

        private void SetAcceptHeader(HttpClient httpClient)
        {
            switch (_config.ResponseType)
            {
                case ResponseType.Json:
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    break;
                case ResponseType.Yaml:
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/yaml"));
                    break;
                case ResponseType.Protobuf:
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.kubernetes.protobuf"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ResponseType));
            }
        }

    }
}
