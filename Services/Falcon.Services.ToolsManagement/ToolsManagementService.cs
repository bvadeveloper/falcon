using System;
using System.Collections.Generic;

namespace Falcon.Services.ToolsManagement
{
    public interface IToolsManagementService
    {
        /// <summary>
        /// Get actual tools in system
        /// </summary>
        /// <returns></returns>
        List<string> GetTools();
    }

    public class ToolsManagementService : IToolsManagementService
    {
        public List<string> GetTools()
        {
            throw new NotImplementedException();
        }
    }
}