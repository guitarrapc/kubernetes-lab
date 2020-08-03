using System;
using System.Collections.Generic;
using System.Text;

namespace KubernetesClient.Models
{
    public class V1NamespaceStatus
    {
        public IList<V1NamespaceCondition> Conditions { get; set; }
        public string Phase { get; set; }
    }
}
