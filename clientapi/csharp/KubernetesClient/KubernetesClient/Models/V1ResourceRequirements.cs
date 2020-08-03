using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1ResourceRequirements
    {
        public IDictionary<string, ResourceQuantity> Limits { get; set; }
        public IDictionary<string, ResourceQuantity> Requests { get; set; }
    }
}
