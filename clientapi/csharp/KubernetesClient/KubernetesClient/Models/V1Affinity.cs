namespace KubernetesClient.Models
{
    public class V1Affinity
    {
        public V1Nodeaffinity NodeAffinity { get; set; }
        public V1Podaffinity PodAffinity { get; set; }
        public V1Podantiaffinity PodAntiAffinity { get; set; }
    }
}
