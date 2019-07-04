using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }

        Task<object> RunToolsAsync(string target);
    }

    public interface IScanToolsModel : IToolsModel
    {
        IScanToolsModel MapOptionalTools(List<string> optionalTools);
    }

    public interface ICollectToolsModel : IToolsModel { }
}