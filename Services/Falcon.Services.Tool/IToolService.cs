using System.Collections.Generic;

namespace Falcon.Services.Tool
{
    public interface IToolService
    {
        List<string> PickupTools(object data);

        List<string> CollectTools();
    }
}