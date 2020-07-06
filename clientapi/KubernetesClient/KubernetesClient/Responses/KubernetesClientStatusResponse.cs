using System;
using System.Collections.Generic;
using System.Text;

namespace KubernetesClient.Responses
{
    public class KubernetesClientStatusResponse
    {
        public string AccessToken { get; set; }

        public string HostName { get; set; }

        public bool IsRunningOnKubernetes { get; set; }

        public string KubernetesServiceEndPoint { get; set; }

        public string Namespace { get; set; }

        public bool SkipCertificationValidation { get; set; }
    }
}
