using System.Threading.Tasks;
using Falcon.Contracts;

namespace Falcon.Services.Scan
{
    public interface IRequestHandler
    {
        Task ProcessAsync(TargetProfile profile);
    }
}