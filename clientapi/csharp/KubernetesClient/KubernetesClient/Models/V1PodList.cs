using System;
using System.Collections.Generic;
using System.Text;

namespace KubernetesClient.Models
{
    public class V1PodList
    {
        public string ApiVersion { get; set; }
        public IList<V1Pod> Items { get; set; }
        public string Kind { get; set; }
        public V1ListMeta Metadata { get; set; }
    }
}
