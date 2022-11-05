using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace VPNConnect.Net
{
    class IpServiceProviderResult
    {
        public string IpAddress { get; set; }
        public bool IsSuccess { get; set; }
    }

    internal class ExternalIpServiceProvider
    {


        private readonly string externalIpServiceLink;

        public ExternalIpServiceProvider(string externalIpServiceLink)
        {
            this.externalIpServiceLink = externalIpServiceLink;
        }


        public IpServiceProviderResult GetMyIp()
        {
            var result = new IpServiceProviderResult();
            try
            {
                var externalIpStringTask = new HttpClient().GetStringAsync(externalIpServiceLink);
                result.IpAddress = externalIpStringTask.Result.Replace("\\r\\n", "").Replace("\\n", "").Trim();
                result.IsSuccess = true;
            }
            catch (HttpRequestException ex)
            {
                Log.Debug($"Can't connect to myIp service {ex}");
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is HttpRequestException)
                    Log.Debug($"Can't connect to myIp service {ex}");
                else throw;
            }
            return result;
        }

    }
}
