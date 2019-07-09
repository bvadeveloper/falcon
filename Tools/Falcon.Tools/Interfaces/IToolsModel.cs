using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }

        Task<List<OutputModel>> RunToolsAsync(string target);

        Task<List<OutputModel>> RunToolsVersionCommandAsync();

        /// <summary>
        /// Set specific tools to toolset
        /// </summary>
        /// <param name="tools"></param>
        /// <returns></returns>
        IScanToolsModel UseOnlyTools(List<string> tools);
    }

    public interface IScanToolsModel : IToolsModel { }

    public interface ICollectToolsModel : IToolsModel { }
}