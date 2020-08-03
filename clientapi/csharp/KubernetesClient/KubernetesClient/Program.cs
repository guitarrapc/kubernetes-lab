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
        private readonly Kubernetes _operations;
        public KubernetesApp(Kubernetes operations)
        {
            _operations = operations;
        }

        public async ValueTask GetOpenApiSpec()
        {
            if (!_operations.IsRunningOnKubernetes)
            {
                Console.WriteLine("App not run on Kubernetes. Quit command.");
                return;
            }

            var res = await _operations.GetOpenApiSpecAsync();
            Console.WriteLine(res);
        }
    }
}
