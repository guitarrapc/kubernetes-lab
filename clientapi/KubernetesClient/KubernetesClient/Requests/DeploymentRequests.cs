using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KubernetesClient.Models;

namespace KubernetesClient.Requests
{
    public class KubernetesDeploymentCreateOrUpdateRequest
    {
        [Required]
        public string NameSpace { get; set; } = "default";
        /// <summary>
        /// Base64 content of Kubernetes Manifest object
        /// </summary>
        [Required]
        public string Body { get; set; }
        public string BodyContentType { get; set; } = "application/yaml";
    }

    public class KubernetesDeploymentDeleteRequest
    {
        [Required]
        public string NameSpace { get; set; } = "default";
        [Required]
        public string Name { get; set; }
    }
    public class V1DeploymentMetadataOnly
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public V1ObjectMeta metadata { get; set; }
        public object spec { get; set; }
    }
}
