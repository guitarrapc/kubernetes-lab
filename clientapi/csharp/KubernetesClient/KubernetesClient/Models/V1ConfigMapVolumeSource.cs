using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1ConfigMapVolumeSource
    {
        public int? DefaultNode { get; set; }
        public IList<V1KeyToPath> Items { get; set; }
        public string Name { get; set; }
    }
}
