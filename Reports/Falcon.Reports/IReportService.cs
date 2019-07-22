using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Profiles;

namespace Falcon.Reports
{
    public interface IReportService
    {
        Task<byte[]> MakeReportAsync(ReportType type, string fileName, List<ReportModel> reportModels);
    }
}