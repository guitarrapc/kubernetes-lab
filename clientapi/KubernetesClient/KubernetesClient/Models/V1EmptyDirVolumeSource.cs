namespace KubernetesClient.Models
{
    public class V1EmptyDirVolumeSource
    {
        public string medium { get; set; }
        public ResourceQuantity sizeLimit { get; set; }
    }
}
