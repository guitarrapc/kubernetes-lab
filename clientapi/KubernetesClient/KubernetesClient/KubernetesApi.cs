using System;
using System.Buffers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KubernetesClient.Models;
using KubernetesClient.Responses;
using LitJWT;
using static KubernetesClient.WatcherDelegatingHandler;

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
        public KubernetesClientStatusResponse GetStatus()
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

        public void ConfigureResponse(bool isJson)
        {
            _config.ResponseType = isJson ? ResponseType.Json : ResponseType.Yaml;
            SetProviderConfig();
        }

        /// <summary>
        /// Get resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        public async ValueTask<string> GetApiAsync(string apiPath, string acceptHeader = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient, acceptHeader);
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
                return res;
            }
        }

        /// <summary>
        /// Get resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="acceptHeader"></param>
        /// <returns></returns>
        public async ValueTask<string> GetStreamApiAsync(string apiPath, string acceptHeader = default, CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient, acceptHeader);
                var request = new HttpRequestMessage(HttpMethod.Get, _provider.KubernetesServiceEndPoint + apiPath);
                var res = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var stream = await res.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var reader = new PeekableStreamReader(new CancelableStream(stream, ct));
                return await reader.ReadLineAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create Resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="body"></param>
        /// <param name="bodyContenType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async ValueTask<string> PostApiAsync(string apiPath, string body, string bodyContenType = "application/yaml",  CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient);
                var content = new StringContent(body, Encoding.UTF8, bodyContenType);
                var res = await httpClient.PostAsync(_provider.KubernetesServiceEndPoint + apiPath, content, ct).ConfigureAwait(false);
                res.EnsureSuccessStatusCode();
                var responseContent = await res.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        /// <summary>
        /// Replace resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="body"></param>
        /// <param name="bodyContenType"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async ValueTask<string> PutApiAsync(string apiPath, string body, string bodyContenType = "application/yaml", CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient);
                var content = new StringContent(body, Encoding.UTF8, bodyContenType);
                var res = await httpClient.PutAsync(_provider.KubernetesServiceEndPoint + apiPath, content, ct).ConfigureAwait(false);
                res.EnsureSuccessStatusCode();
                var responseContent = await res.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        /// <summary>
        /// Delete resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async ValueTask<string> DeleteApiAsync(string apiPath, CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient, "application/json");
                var res = await httpClient.DeleteAsync(_provider.KubernetesServiceEndPoint + apiPath, ct);
                res.EnsureSuccessStatusCode();
                var responseContent = await res.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
        /// <summary>
        /// Delete resource
        /// </summary>
        /// <param name="apiPath"></param>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async ValueTask<string> DeleteApiAsync(string apiPath, V1DeleteOptions options, CancellationToken ct = default)
        {
            using (var httpClient = _provider.CreateHttpClient())
            {
                SetAcceptHeader(httpClient, "application/json");
                var content = new StringContent(JsonSerializer.Serialize(options), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Delete, _provider.KubernetesServiceEndPoint + apiPath)
                {
                    Content = content,
                };
                var res = await httpClient.SendAsync(request, ct);
                res.EnsureSuccessStatusCode();
                var responseContent = await res.Content.ReadAsStringAsync();
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
                var res = await httpClient.GetStringAsync(_provider.KubernetesServiceEndPoint + apiPath).ConfigureAwait(false);
                return res;
            }
        }
        #endregion

        public static string Base64ToString(string base64)
        {
            var rentBytes = ArrayPool<byte>.Shared.Rent(Base64.GetMaxBase64UrlDecodeLength(base64.Length));
            try
            {
                Span<byte> bytes = rentBytes.AsSpan();
                if (!Base64.TryFromBase64String(base64, bytes, out var bytesWritten))
                {
                    throw new ArgumentException($"Invalid Base64 string detected.");
                }
                bytes = bytes.Slice(0, bytesWritten);
                return UTF8Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rentBytes);
            }
        }

        private static IKubernetesClient GetDefaultProvider()
        {
            return Environment.OSVersion.Platform == PlatformID.Unix
                ? (IKubernetesClient)new UnixKubernetesClient()
                : (IKubernetesClient)new WindowsKubernetesClient();
        }
        private void SetProviderConfig()
        {
            _provider.SkipCertificationValidation = _config.SkipCertificateValidation;
        }

        private void SetAcceptHeader(HttpClient httpClient, string acceptHeader)
        {
            if (string.IsNullOrEmpty(acceptHeader))
            {
                SetAcceptHeader(httpClient);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));
            }
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
