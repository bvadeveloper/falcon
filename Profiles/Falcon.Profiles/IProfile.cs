using System.Collections.Generic;

namespace Falcon.Profiles
{
    public interface IProfile : ISessionContext
    {
        string Target { get; set; }

        List<string> Tools { get; set; }
    }
}