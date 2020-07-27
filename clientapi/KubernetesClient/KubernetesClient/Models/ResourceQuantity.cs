using System.Text.Json.Serialization;
using KubernetesClient.Converters;

namespace KubernetesClient.Models
{
    [JsonConverter(typeof(ResourceQuantityConverter))]
    public class ResourceQuantity
    {
        public string value { get; set; }
    }
}
