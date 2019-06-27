using System.Collections.Generic;

namespace Falcon.Scripts
{
    public class Script
    {
        public string Target { get; set; }

        public Queue<string> ProcessingQueue { get; set; }
    }
}