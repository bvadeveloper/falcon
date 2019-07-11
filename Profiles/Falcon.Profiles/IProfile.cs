using System.Collections.Generic;

namespace Falcon.Profiles
{
    public interface ITargetProfile : ISession
    {
        string Target { get; set; }
    }

    public interface IToolProfile : ISession
    {
        List<string> Tools { get; set; }
    }

    public interface IReportProfile : ITargetProfile
    {
        List<ReportModel> Reports { get; set; }
    }
}