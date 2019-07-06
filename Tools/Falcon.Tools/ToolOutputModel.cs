using System;

namespace Falcon.Tools
{
    public class ToolOutputModel
    {
        public string Output { get; set; }

        public string ErrorOutput { get; set; }

        public string ToolName { get; set; }
        
        public Exception ExecutionException { get; set; }
        
        public bool Successful { get; set; }
    }
}