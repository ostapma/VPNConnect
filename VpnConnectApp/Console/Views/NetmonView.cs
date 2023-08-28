using ExternalPing;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Views
{
    internal class NetmonData
    {
        public NetmonData(int resultBufferSize)
        {
            PingResults = new (PingResult vpnPing, PingResult bypassPing)?[resultBufferSize];
        }
        public DateTime TimeStamp { get; set; }
        public (PingResult vpnPing, PingResult bypassPing )?[] PingResults { get; private set; }
        public int TolerableLatency { get; set; }
    }

    internal class NetmonView
    {
        private const int RefreshRateMs = 1000;
        private readonly int rowsToShow;

        public NetmonView(NetmonData dataToShow, int rowsToShow)
        {
           this.dataToShow = dataToShow;
            this.rowsToShow = rowsToShow;
        }

        public event Action OnStop;

        private NetmonData dataToShow { get; set; }

        public void ShowMonitor()
        {
            var table = new Table().LeftAligned();
            table.Border(TableBorder.Horizontal);
            table.AddColumn(new TableColumn("Time"));
            table.AddColumn(new TableColumn("|"));
            table.AddColumn(new TableColumn("Latency Vpn"));
            table.AddColumn(new TableColumn("|"));
            table.AddColumn(new TableColumn("Latency Bypass"));

            for (int i = 0; i < rowsToShow; i++)
            {
                table.AddEmptyRow();
            }


            AnsiConsole.Live(table).Start(
                ctx =>
                {
                    ctx.Refresh();
                    DateTime lastUpdateTime = DateTime.MinValue;
                    while (true)
                    {
                        if (dataToShow != null)
                        {
                            //if (lastUpdateTime != dataToShow.TimeStamp)
                            {

                                int i = 0;
                                foreach (var result in dataToShow.PingResults.Where(pr=>pr is not null).ToList()
                                    .OrderByDescending(r => r.Value.vpnPing.PingTime).Take(rowsToShow))
                                {
                                    if (i == 0)
                                    {
                                        table.UpdateCell(i, 0, $"[green]{result.Value.vpnPing.PingTime}[/]");
                                        table.UpdateCell(i, 2, $"[green]{GetLatencyStr(result.Value.vpnPing)}[/]");
                                        table.UpdateCell(i, 4, $"[green]{GetLatencyStr(result.Value.bypassPing)}[/]");
                                    }
                                    else
                                    {
                                        table.UpdateCell(i, 0, result.Value.vpnPing.PingTime.ToString());
                                        table.UpdateCell(i, 2, GetLatencyStr(result.Value.vpnPing));
                                        table.UpdateCell(i, 4, GetLatencyStr(result.Value.bypassPing));
                                    }
                                    i++;
                                }

                                lastUpdateTime = dataToShow.TimeStamp;
                                ctx.Refresh();

                            }

                        }
                        Thread.Sleep(RefreshRateMs);
                    }
                });
        }

        private string GetLatencyStr(PingResult result)
        {
            return result.IsSuccess ? result.PingLatency.ToString() : result.Error;
        }

    }
}

