using System;
using System.Net.Http;

namespace KubernetesClient.Responses
{
    public class HttpResponse: IDisposable
    {
        public HttpResponseMessage Response { get; set; }

        public void Dispose()
        {
            Response?.Content?.Dispose();
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
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(HttpResponseWrapper left, HttpResponseWrapper right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HttpResponseWrapper left, HttpResponseWrapper right)
        {
            return !(left == right);
        }
    }
}
