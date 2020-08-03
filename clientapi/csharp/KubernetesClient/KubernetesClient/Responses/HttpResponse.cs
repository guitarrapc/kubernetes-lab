using System;
using System.Net.Http;

namespace KubernetesClient.Responses
{
    public class HttpResponse: IDisposable
    {
        public HttpResponseMessage Response { get; set; }

        public void Dispose()
        {
            Response?.Dispose();
        }
    }

    public class HttpResponse<T> : HttpResponse
    {
        public T Body { get; }

        public HttpResponse(T body)
        {
            Body = body;
        }
    }

    internal readonly struct HttpResponseWrapper
    {
        public readonly HttpResponseMessage HttpResponseMessage;
        public readonly string Content;

        public HttpResponseWrapper(HttpResponseMessage httpResponseMessage, string content)
        {
            HttpResponseMessage = httpResponseMessage;
            Content = content;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
