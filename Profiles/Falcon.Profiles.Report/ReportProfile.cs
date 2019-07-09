using System.Collections.Generic;

namespace Falcon.Profiles.Report
{
    public class ReportProfile : ITargetProfile
    {
        public SessionContext Context { get; set; }
        
        public string Target { get; set; }
        
        public List<ReportModel> Reports { get; set; }
    }
}