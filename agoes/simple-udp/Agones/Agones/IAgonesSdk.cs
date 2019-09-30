using System.Threading.Tasks;

namespace Agones
{
    public interface IAgonesSdk
    {
        Task<bool> Allocate();
        Task<bool> Ready();
        Task<bool> SetAnnotation(string key, string value);
        Task<bool> SetLabel(string key, string value);
        Task<bool> Shutdown();
    }
}