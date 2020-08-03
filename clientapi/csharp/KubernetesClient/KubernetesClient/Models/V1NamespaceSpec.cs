using System;
using System.Collections.Generic;
using System.Text;

namespace KubernetesClient.Models
{
    public class V1NamespaceSpec
    {
        public IList<string> Finalizers { get; set; }
    }
}
