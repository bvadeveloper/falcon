using System.Threading.Tasks;
using Falcon.Profiles.Scan;

namespace Falcon.Hosts
{
    public interface IRequestHandler
    {
        Task ProcessAsync(ScanDomainProfile profile);
    }
}