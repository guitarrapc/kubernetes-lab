using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KubernetesClient.Models
{
    public class GenericKubernetesObject
    {
        /// <summary>
        /// Kubernetes ApiVersion
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#resources
        /// </summary>
        [JsonPropertyName("apiVersion")]
        public string ApiVersion { get; set; }

        /// <summary>
        /// Kubernetes Kind
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#types-kinds
        /// </summary>
        [JsonPropertyName("kind")]
        public string Kind { get; set; }
    }
}
