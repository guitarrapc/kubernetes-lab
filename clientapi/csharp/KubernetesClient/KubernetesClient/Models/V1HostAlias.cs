using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1HostAlias
    {
        public IList<string> Hostnames { get; set; }
        public string Ip { get; set; }
    }
}
