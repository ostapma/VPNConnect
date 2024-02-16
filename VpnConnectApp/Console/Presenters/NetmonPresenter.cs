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
        NetmonView view;
        bool isStarted = false;
        const int latencyRows = 10;
        const int dataBufferSize = 100;
        NetmonData data = new NetmonData(dataBufferSize);

        public NetmonPresenter(string pingTarget)
        {
            view =  new NetmonView(data, latencyRows);
            this.pingTarget = pingTarget;
        }

        public void Monitor()
        {
            isStarted = true;
            Task.Factory.StartNew(() =>
            {
                IPinger icmpPinger = new IcmpPinger(pingTarget);
                TcpPingerWrapper pingWrapperBypass = new TcpPingerWrapper("Bypass", pingTarget, 80);
                icmpPinger.StartPinging();
                pingWrapperBypass.StartPinging();
                while (isStarted)
                {
                    data.TimeStamp= DateTime.Now;

                    
                    for (int i = data.PingResults.Length-1; i > 0; i--)
                    {
                        data.PingResults[i] = data.PingResults[i-1];

                    }

                    data.PingResults[0] =(icmpPinger.GetPingResult(), pingWrapperBypass.GetPingResult());

                    Thread.Sleep(1000);
                }
                icmpPinger.StopPinging();
                pingWrapperBypass.StopPinging();
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
