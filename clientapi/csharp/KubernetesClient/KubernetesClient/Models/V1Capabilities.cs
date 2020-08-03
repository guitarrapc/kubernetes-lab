using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1Capabilities
    {
        public IList<string> Add { get; set; }
        public IList<string> Drop { get; set; }
    }
}
