using System.Text.Json.Serialization;
using KubernetesClient.Serializers;

namespace KubernetesClient.Models
{
    public class V1TCPSocketAction
    {
        public string Host { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string Port { get; set; }
    }
}
