using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1ResourceRequirements
    {
        public IDictionary<string, ResourceQuantity> limits { get; set; }
        public IDictionary<string, ResourceQuantity> requests { get; set; }
    }
}
