using System;
using System.Collections.Generic;
using System.Text;

namespace KubernetesClient.Models
{
    public class V1WatchEvent
    {
        public object @object { get; set; }
        public string type { get; set; }
    }
}
