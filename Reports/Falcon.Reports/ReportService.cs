using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Falcon.Profiles;

namespace Falcon.Reports
{
    public class ReportService : IReportService
    {
        public Task<(string, byte[])> MakeFileReportAsync(string target, List<ReportModel> models)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"target: {target}");

            models.ForEach(m =>
            {
                sb.AppendLine();
                sb.AppendLine($"processing date: {m.ProcessingDate}");
                sb.AppendLine($"tool name: {m.ToolName}");
                sb.AppendLine();
                sb.AppendLine(m.Output);
            });

            var fileName = $"{target}_report_{DateTime.UtcNow:yyMMddHHmm}.txt";
            var report = Encoding.Default.GetBytes(sb.ToString());

            return Task.FromResult((fileName, report));
        }

        public Task<string> MakeTextReportAsync(string target, List<ReportModel> models)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"target: {target}");

            models.ForEach(m =>
            {
                sb.AppendLine();
                sb.AppendLine($"processing date: {m.ProcessingDate}");
                sb.AppendLine($"tool name: {m.ToolName}");
                sb.AppendLine();
                sb.AppendLine(m.Output);
            });

            return Task.FromResult(sb.ToString());
        }
    }
}