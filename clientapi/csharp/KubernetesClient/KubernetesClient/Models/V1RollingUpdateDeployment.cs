using System.Text.Json.Serialization;
using KubernetesClient.Serializers;

namespace KubernetesClient.Models
{
    public class V1RollingUpdateDeployment
    {
        [JsonConverter(typeof(IntOrStringConverter))]

        public string MaxSurge { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string MaxUnavailable { get; set; }
    }
}
