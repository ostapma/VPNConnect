using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;

namespace VpnConnect.Net
{
    internal class IcmpPinger : IPinger
    {
        private string pingTarget;
        private string ip;

        public IcmpPinger(string pingTarget)
        {
            this.pingTarget = pingTarget;
        }

        PingResult IPinger.GetPingResult()
        {
            PingResult pingResult = new PingResult { PingTime = DateTime.Now, Ip = ip};
            Ping ping = new Ping();
            PingReply? pingReply;
            try
            {
                pingReply = ping.Send(pingTarget);
                pingResult.IsSuccess = pingReply.Status == IPStatus.Success;
                if (pingResult.IsSuccess)
                {
                    pingResult.PingLatency = (int)pingReply.RoundtripTime;
                }
                else pingResult.Error = pingReply.Status.ToString();
            }

            catch (PingException ex)
            {
                pingResult.IsSuccess = false;
                pingResult.PingLatency = 0;
                pingResult.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return pingResult;
        }

        void IPinger.StartPinging()
        {
            var hostEntry = Dns.GetHostEntry(pingTarget);
            if (hostEntry.AddressList.Length>0)
                ip = hostEntry.AddressList[0].ToString();
        }

        void IPinger.StopPinging()
        {
            
        }
    }
}
