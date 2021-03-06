﻿using System.Collections.Generic;

namespace KubernetesClient.Models
{
    public class V1PodDNSConfig
    {
        public IList<string> Nameservers { get; set; }
        public IList<V1PodDNSConfigOption> Options { get; set; }
        public IList<string> Searches { get; set; }
    }
}
