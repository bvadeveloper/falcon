using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Profiles;

namespace Falcon.Services.RequestProcessing
{
    public class RequestProcessingService : IRequestProcessingService
    {
        public Task<Result> ScanIpAsync(string ip)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ScanDomainsAsync(List<string> domains, List<string> tools)
        {
            throw new NotImplementedException();
        }

        public Task<Result<string>> ScanEmailsAsync(List<string> emails)
        {
            return Task.FromResult(new Result<string>().SetResult("your emails is hacked"));
        }

        public Task<Result<string>> ScanGdprInfoAsync(string domain)
        {
            throw new NotImplementedException();
        }
    }
}