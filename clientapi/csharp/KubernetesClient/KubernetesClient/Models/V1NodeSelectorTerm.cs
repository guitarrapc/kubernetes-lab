using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1NodeSelectorTerm
    {
        public IList<V1NodeSelectorRequirement> MatchExpressions { get; set; }
        public IList<V1NodeSelectorRequirement> MatchFields { get; set; }
    }
}
