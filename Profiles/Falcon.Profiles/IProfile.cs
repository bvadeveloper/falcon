using System.Collections.Generic;

namespace Falcon.Profiles
{
    public interface ITargetProfile : ISessionContext
    {
        string Target { get; set; }
    }

    public interface IToolProfile : ISessionContext
    {
        List<string> Tools { get; set; }
    }

    public interface IReportProfile : ITargetProfile
    {
        List<ReportModel> Reports { get; set; }
    }
}