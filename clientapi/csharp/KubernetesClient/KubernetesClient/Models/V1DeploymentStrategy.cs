namespace KubernetesClient.Models
{
    public class V1DeploymentStrategy
    {
        public V1RollingUpdateDeployment RollingUpdate { get; set; }
        public string Type { get; set; }
    }
}
