using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }

        Task<List<OutputModel>> RunToolsAsync(string target);

        Task<List<OutputModel>> RunToolsVersionAsync();

        IToolsModel UseOnly(List<string> tools);
    }
}