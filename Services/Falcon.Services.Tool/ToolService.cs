using System.Collections.Generic;

namespace Falcon.Services.Tool
{
    public class ToolService : IToolService
    {
        public List<string> PickupTools(object data)
        {
            // Analyze target collected data
            // Pickup appropriate tools
            // If no tools are found, return the default tools

            return new List<string>();
        }

        public List<string> CollectTools()
        {
            // returns collection of tools for collecting data about target
            
            return new List<string>();
        }
    }
}