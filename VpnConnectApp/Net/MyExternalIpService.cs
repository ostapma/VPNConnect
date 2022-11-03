using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace VPNConnect.Net
{
    internal class ExternalIpServiceProvider
    {
        private readonly string externalIpServiceLink;

        public ExternalIpServiceProvider(string externalIpServiceLink)
        {
            this.externalIpServiceLink = externalIpServiceLink;
        }


        public string GetMyIp()
        {
            try
            {
                var externalIpStringTask = new HttpClient().GetStringAsync(externalIpServiceLink);
                return externalIpStringTask.Result.Replace("\\r\\n", "").Replace("\\n", "").Trim();
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
            return String.Empty;            
        }

    }
}
