using System.Collections.Generic;

namespace Falcon.Profiles.Report
{
    public class ReportProfile : IReportProfile
    {
        public IContext Context { get; set; }
        
        public string Target { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}