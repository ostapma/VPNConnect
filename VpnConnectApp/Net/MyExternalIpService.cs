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
            var externalIpStringTask = new HttpClient().GetStringAsync(externalIpServiceLink);

            return externalIpStringTask.Result.Replace("\\r\\n", "").Replace("\\n", "").Trim();
        }

    }
}
