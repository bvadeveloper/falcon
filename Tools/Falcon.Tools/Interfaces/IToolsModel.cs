using System.Collections.Generic;

namespace Falcon.Tools.Interfaces
{
    public interface IToolsModel
    {
        List<ToolModel> Toolset { get; set; }
    }

    public interface IScanToolsModel : IToolsModel { }

    public interface ICollectToolsModel : IToolsModel { }
}