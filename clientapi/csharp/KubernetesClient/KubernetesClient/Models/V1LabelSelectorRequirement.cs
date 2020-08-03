using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1LabelSelectorRequirement
    {
        public string Key { get; set; }
        public string Operator { get; set; }
        public IList<string> Values { get; set; }
    }
}
