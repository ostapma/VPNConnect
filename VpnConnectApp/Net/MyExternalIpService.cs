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
        public bool IsSuccess { get; set; } = false;

        public bool IsTimeout { get; set; } = false;
    }

    internal class ExternalIpServiceProvider
    {
        private const int defaultTimeoutSec = 100;

        private readonly string externalIpServiceLink;

        public ExternalIpServiceProvider(string externalIpServiceLink)
        {
            this.externalIpServiceLink = externalIpServiceLink;
        }


        public IpServiceProviderResult WaitForMyIpChanging(string initialIp, int timeoutSec = defaultTimeoutSec)
        {
            bool checkIpTaskStop = false;
            var ip = new IpServiceProviderResult { };
            var checkIpTask = Task.Run(() =>
            {
                while (!checkIpTaskStop)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    ip = GetMyIp();
                    if (ip.IsSuccess && ip.IpAddress != initialIp)
                    {
                        checkIpTaskStop = true;
                    }
                }
            });
            if (!checkIpTask.Wait(TimeSpan.FromSeconds(timeoutSec)))
            {
                checkIpTaskStop = true;
                ip.IsTimeout = true;
                ip.IsSuccess = false;
            };

            return ip;
        }

        public IpServiceProviderResult GetMyIp()
        {
            var result = new IpServiceProviderResult();
            try
            {
                HttpClient httpClient = new HttpClient();
                var externalIpStringTask = httpClient.GetStringAsync(externalIpServiceLink);
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
