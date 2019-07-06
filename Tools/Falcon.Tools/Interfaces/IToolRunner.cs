using System.Threading;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolRunner
    {
        /// <summary>
        /// Make tool runner tasks
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ToolRunner> MakeTask(CancellationToken token);

        /// <summary>
        /// Init tool runner with tool name and command line params
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        ToolRunner Init(string name, string commandLine);

        /// <summary>
        /// Make runner output
        /// </summary>
        /// <returns></returns>
        ToolOutputModel MakeOutput();
    }
}