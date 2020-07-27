using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1PodAffinityTerm
    {
        public V1LabelSelector labelSelector { get; set; }
        public IList<string> namespaces { get; set; }
        public string topologyKey { get; set; }
    }
}
