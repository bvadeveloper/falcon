using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolRunner : IToolRunner
    {
        private bool Successful { get; set; } = true;

        private Exception ExecutionException { get; set; }

        private string ToolName { get; set; }

        private string CommandLine { get; set; }

        private string Output { get; set; }

        private string ErrorOutput { get; set; }


        public OutputModel MakeOutput() => new OutputModel
        {
            ToolName = ToolName,
            Output = Output,
            ErrorOutput = ErrorOutput,
            ExecutionException = ExecutionException,
            Successful = Successful
        };


        public Task<ToolRunner> MakeTask(CancellationToken token) =>
            Task.Run(() =>
            {
                try
                {
                    using (var process = new Process())
                    {
                        process.StartInfo = InitStartInfo(CommandLine);
                        process.Start();
                        Output += process.StandardOutput.ReadToEnd();
                        ErrorOutput += process.StandardError.ReadToEnd();
                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    ErrorOutput += Environment.NewLine + CommandLine + Environment.NewLine + ex.InnerException.Message;
                    ExecutionException = ex;
                    Successful = false;
                }

                return this;
            }, token);


        private static ProcessStartInfo InitStartInfo(string command) =>
            new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

        public ToolRunner Init(string toolName, string commandLine)
        {
            if (toolName == null)
                throw new ArgumentNullException(nameof(toolName));

            if (commandLine == null)
                throw new ArgumentNullException(nameof(commandLine));

            ToolName = toolName;
            CommandLine = commandLine;

            return this;
        }
    }
}