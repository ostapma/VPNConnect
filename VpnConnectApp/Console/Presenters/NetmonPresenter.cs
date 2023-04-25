using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Views;

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
            view.OnStop += OnStop;
        }

        public void Monitor()
        {
            isStarted = true;
            Task.Factory.StartNew(() =>
            {
                while (isStarted)
                {
                    data.TimeStamp= DateTime.Now;
                    PingResult pingResult = new PingResult();
                    pingResult.PingTime = DateTime.Now;
                    Ping ping = new Ping();
                    PingReply? result;
                    try
                    {
                        result = ping.Send(pingTarget);
                        pingResult.IsSuccess = result.Status == IPStatus.Success;
                        if (pingResult.IsSuccess)
                        {
                            pingResult.PingLatency = (int)result.RoundtripTime;
                        }
                        else pingResult.Error = result.Status.ToString();
                    }

                    catch (PingException ex) {
                        pingResult.IsSuccess=false;
                        pingResult.PingLatency = 0;
                        pingResult.Error = ex.InnerException!=null?ex.InnerException.Message : ex.Message;
                    }
                    
                    for (int i = data.PingResults.Length-1; i > 0; i--)
                    {
                        data.PingResults[i] = data.PingResults[i-1];

                    }
                    data.PingResults[0] = pingResult;
                    Thread.Sleep(1000);
                }
            });

            view.ShowMonitor();
        }

        private void OnStop()
        {
            isStarted= false;
        }
    }
}
