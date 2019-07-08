using System.Collections.Generic;

namespace Falcon.Profiles.Report
{
    public class CollectReportProfile : ITargetProfile
    {
        public SessionContext Context { get; set; }
        
        public string Target { get; set; }
        
        public List<ReportModel> ReportModels { get; set; }
    }

    public class ReportModel
    {
        public string Output { get; set; }

        public string ToolName { get; set; }
    }
}