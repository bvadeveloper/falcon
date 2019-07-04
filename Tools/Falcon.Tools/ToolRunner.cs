using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolRunner : IToolRunner
    {
        private string Output { get; set; }

        private string ErrorOutput { get; set; }

        public string GetOutput() => Output;

        public string GetErrorOutput() => ErrorOutput;

        public Task<ToolRunner> MakeTask(string command, CancellationToken token) =>
            Task.Run(() =>
            {
                using (var process = new Process())
                {
                    process.StartInfo = MakeStartInfo(command);
                    process.Start();
                    Output += process.StandardOutput.ReadToEnd();
                    ErrorOutput += process.StandardError.ReadToEnd(); // todo: ??
                    process.WaitForExit();
                }

                return this;
            }, token);

        private static ProcessStartInfo MakeStartInfo(string command) =>
            new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
    }
}