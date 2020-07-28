using System;
using System.Net.Http;

namespace KubernetesClient.Responses
{
    internal struct HttpResponseTuple
    {
        public HttpResponseMessage HttpResponseMessage;
        public string Content;

        public HttpResponseTuple(HttpResponseMessage httpResponseMessage, string content)
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

        public static bool operator ==(HttpResponseTuple left, HttpResponseTuple right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HttpResponseTuple left, HttpResponseTuple right)
        {
            return !(left == right);
        }
    }
}
