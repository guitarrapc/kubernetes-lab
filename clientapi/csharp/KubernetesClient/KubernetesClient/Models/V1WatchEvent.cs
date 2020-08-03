namespace KubernetesClient.Models
{
    public class V1WatchEvent<T>
    {
        public T @object { get; set; }
        public string type { get; set; }
    }
}
