using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1NodeSelector
    {
        public IList<V1NodeSelectorTerm> nodeSelectorTerms { get; set; }
    }
}
