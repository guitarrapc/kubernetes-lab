using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KubernetesClient.Models;

namespace KubernetesClient.Requests
{
    public class KubernetesCreateOrUpdateRequest
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

    public class KubernetesDeleteRequest
    {
        [Required]
        public string NameSpace { get; set; } = "default";
        [Required]
        public string Name { get; set; }
        public int? GraceperiodSecond { get; set; }
    }
    public class V1MetadataOnly
    {
        [Required]
        public string ApiVersion { get; set; }
        [Required]
        public string Kind { get; set; }
        [Required]
        public V1ObjectMeta Metadata { get; set; }
        public object Spec { get; set; }
    }
}
