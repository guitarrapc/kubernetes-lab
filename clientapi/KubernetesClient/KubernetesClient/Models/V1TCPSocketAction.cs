using System.Text.Json.Serialization;
using KubernetesClient.Serializers;

namespace KubernetesClient.Models
{
    public class V1TCPSocketAction
    {
        public string host { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string port { get; set; }
    }
}
