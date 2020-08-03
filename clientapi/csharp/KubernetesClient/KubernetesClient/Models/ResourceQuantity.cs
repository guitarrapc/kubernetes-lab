using System.Text.Json.Serialization;
using KubernetesClient.Serializers;

namespace KubernetesClient.Models
{
    [JsonConverter(typeof(ResourceQuantityConverter))]
    public class ResourceQuantity
    {
        public string Value { get; set; }
    }
}
