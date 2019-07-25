using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Profiles;

namespace Falcon.Reports
{
    public interface IReportService
    {
        Task<(string, byte[])> MakeFileReportAsync(string target, List<ReportModel> models);

        Task<string> MakeTextReportAsync(string target, List<ReportModel> models);
    }
}