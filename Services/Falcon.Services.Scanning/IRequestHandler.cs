using System.Threading.Tasks;
using Falcon.Scripts;

namespace Falcon.Services.Scanning
{
    public interface IRequestHandler
    {
        Task ProcessAsync(Script script);
    }
}