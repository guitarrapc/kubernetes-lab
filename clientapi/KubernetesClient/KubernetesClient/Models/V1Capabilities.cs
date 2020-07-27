using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1Capabilities
    {
        public IList<string> add { get; set; }
        public IList<string> drop { get; set; }
    }
}
