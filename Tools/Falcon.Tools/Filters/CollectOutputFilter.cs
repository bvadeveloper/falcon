using System.Collections.Generic;
using Falcon.Profiles;

namespace Falcon.Tools.Filters
{
    public interface ICollectOutputFilter
    {
        TargetAttributes ApplyFilter(List<OutputModel> outputs);

        List<string> ApplyVersionFilter(List<OutputModel> outputs);
    }

    public class CollectOutputFilterTool1 : ICollectOutputFilter { }

    public class CollectOutputFilterTool2 : ICollectOutputFilter { }

    public class CollectOutputFilterTool3 : ICollectOutputFilter { }

    public class CollectOutputFilterTool4 : ICollectOutputFilter { }

    public class CollectOutputFilterTool5 : ICollectOutputFilter { }
}