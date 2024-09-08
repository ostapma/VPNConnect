using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Views;
using VpnConnect.Net;

namespace VpnConnect.Console.Presenters
{
    internal class NetmonPresenter
    {
        private readonly string pingTarget;
        private readonly bool pingBypass;
        NetmonView view;
        bool isStarted = false;
        const int latencyRows = 10;
        const int dataBufferSize = 10;
        NetmonData data = new NetmonData(dataBufferSize);

        public NetmonPresenter(string pingTarget, bool pingBypass)
        {
            view =  new NetmonView(data, latencyRows, pingBypass);
            this.pingTarget = pingTarget;
            this.pingBypass = pingBypass;
        }

        public void Monitor()
        {
            isStarted = true;
            Task.Factory.StartNew(() =>
            {
                List<IPinger> pingers = [new IcmpPinger(pingTarget)];
                if (pingBypass) pingers.Add(new TcpPingerWrapper("Bypass", pingTarget, 80));
                foreach (IPinger pinger in pingers) {
                    pinger.StartPinging();
                }
                while (isStarted)
                {
                    data.TimeStamp = DateTime.Now;
                 
                    var pingResults = new List<VPNConnect.Net.PingResult>();
                    foreach (IPinger pinger in pingers) {
                        pingResults.Add (pinger.GetPingResult());
                    }
                    data.PingResults.Add(pingResults);

                    if (data.PingResults.Count >= dataBufferSize)
                        data.PingResults.Take();

                    Thread.Sleep(1000);
                }
                foreach (IPinger pinger in pingers)
                {
                    pinger.StopPinging();
                }
            });

            view.ShowMonitor(Stop);
        }

        public void Stop()
        {
            isStarted = false;
            view.Stop();
        }
    }
}
