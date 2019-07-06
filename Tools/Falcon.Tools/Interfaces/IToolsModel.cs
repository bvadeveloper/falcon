using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }

        Task<IEnumerable<OutputModel>> RunToolsAsync(string target);
        
        Task<IEnumerable<OutputModel>> RunToolsVersionCommandAsync();
    }

    public interface IScanToolsModel : IToolsModel
    {
        /// <summary>
        /// Use specific tools for scanners
        /// </summary>
        /// <param name="tools"></param>
        /// <returns></returns>
        IScanToolsModel UseOnlyTools(List<string> tools);
    }

    public interface ICollectToolsModel : IToolsModel { }
}