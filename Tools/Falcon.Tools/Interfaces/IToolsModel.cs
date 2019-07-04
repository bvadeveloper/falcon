using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }

        Task<List<string>> RunToolsAsync(string target);
    }

    public interface IScanToolsModel : IToolsModel
    {
        /// <summary>
        /// Map optional tools only for scanners
        /// </summary>
        /// <param name="optionalTools"></param>
        /// <returns></returns>
        IScanToolsModel MapOptionalTools(List<string> optionalTools);
    }

    public interface ICollectToolsModel : IToolsModel { }
}