using System;
using System.Runtime.Serialization;
using KubernetesClient.Models;

namespace KubernetesClient
{
    public class KubernetesException : Exception
    {
        public V1Status Status { get; private set; }

        public KubernetesException()
        {
        }
        public KubernetesException(V1Status status)
            : this(status?.message)
        {
            this.Status = status;
        }

        public KubernetesException(string message)
            : base(message)
        {
        }

        public KubernetesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected KubernetesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
