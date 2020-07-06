using System;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;

namespace KubernetesClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync<KubernetesApp>(args);
        }
    }

    public class KubernetesApp : ConsoleAppBase
    {
        private KubernetesApi api;
        public KubernetesApp(KubernetesApi api)
        {
            this.api = new KubernetesApi();
        }

        public async ValueTask GetOpenApiSpec()
        {
            if (!api.IsRunningOnKubernetes)
            {
                Console.WriteLine("App not run on Kubernetes. Quit command.");
                return;
            }

            var res = await api.GetOpenApiSpecAsync();
            Console.WriteLine(res);
        }
    }
}
