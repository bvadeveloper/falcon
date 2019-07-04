using System.Collections.Generic;

namespace Falcon.Tools.Interfaces
{
    public interface ITools
    {
        List<ITool> ToolCollection { get; set; }
    }
}