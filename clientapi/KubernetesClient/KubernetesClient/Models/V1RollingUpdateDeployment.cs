using System.Text.Json.Serialization;
using KubernetesClient.Converters;

namespace KubernetesClient.Models
{
    public class V1RollingUpdateDeployment
    {
        [JsonConverter(typeof(IntOrStringConverter))]

        public string maxSurge { get; set; }
        [JsonConverter(typeof(IntOrStringConverter))]

        public string maxUnavailable { get; set; }
    }
}
