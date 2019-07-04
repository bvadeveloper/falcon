using System.Threading;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolRunner
    {
        Task<ToolRunner> MakeTask(string command, CancellationToken token);

        string GetOutput();

        string GetErrorOutput();
    }
}