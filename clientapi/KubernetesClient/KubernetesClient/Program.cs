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
        private Kubernetes api;
        public KubernetesApp(Kubernetes api)
        {
            this.api = new Kubernetes();
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
