using System.Threading.Tasks;
using Falcon.Contracts;

namespace Falcon.Services.Scanning
{
    public interface IRequestHandler
    {
        Task ProcessAsync(TargetProfile script);
    }
}