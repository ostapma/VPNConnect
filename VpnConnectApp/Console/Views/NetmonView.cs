using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;

namespace VpnConnect.Console.Views
{
    internal class NetmonData
    {
        public NetmonData(int resultBufferSize)
        {
            PingResults = new List<PingResult>[resultBufferSize];
        }
        public DateTime TimeStamp { get; set; }
        public List<PingResult>[] PingResults { get; private set; }
        public int TolerableLatency { get; set; }
    }

    internal class NetmonView
    {
        private const int RefreshRateMs = 1000;
        private readonly int rowsToShow;
        private readonly bool showBypass;
        private bool isActive;

        public NetmonView(NetmonData dataToShow, int rowsToShow, bool showBypass)
        {
            this.dataToShow = dataToShow;
            this.rowsToShow = rowsToShow;
            this.showBypass = showBypass;
        }




        private NetmonData dataToShow { get; set; }

        public void ShowMonitor(Action onStop)
        {
            isActive = true;
            AnsiConsole.MarkupLine("[green]Press ESC to stop[/]");

            new Task(() => {
                while (System.Console.ReadKey(false).Key!=ConsoleKey.Escape);
                    onStop();
                }).Start();

            var rootTable = new Table().LeftAligned().NoBorder();
            rootTable.AddColumn("");

            var table = new Table().LeftAligned();
            table.Border(TableBorder.Horizontal);
            table.AddColumn(new TableColumn("Time"));
            table.AddColumn(new TableColumn("|"));
            table.AddColumn(new TableColumn("Latency Direct"));
            if (showBypass)
            {
                table.AddColumn(new TableColumn("|"));
                table.AddColumn(new TableColumn("Latency Bypass"));
            }

            for (int i = 0; i < rowsToShow; i++)
            {
                table.AddEmptyRow();
            }

            rootTable.AddRow(table);
            rootTable.AddEmptyRow();
            rootTable.AddEmptyRow();

            LiveDisplay liveDisplay = AnsiConsole.Live(rootTable);
            liveDisplay.AutoClear = true;
            liveDisplay.Start(
                ctx =>
                {
                    ctx.Refresh();
                    DateTime lastUpdateTime = DateTime.MinValue;
                    while (isActive)
                    {
                        if (dataToShow != null)
                        {
                            //if (lastUpdateTime != dataToShow.TimeStamp)
                            {

                                int i = 0;
                                foreach (var result in dataToShow.PingResults.Where(pr => pr is not null && pr.Count>0).ToList()
                                    .OrderByDescending(r => r.First().PingTime).Take(rowsToShow))
                                {
                                    if (i == 0)
                                    {
                                        table.UpdateCell(i, 0, $"[white]{result.First().PingTime}[/]");
                                        table.UpdateCell(i, 2, $"[green]{GetLatencyStr(result.First())}[/]");
                                        if(showBypass) table.UpdateCell(i, 4, $"[blue]{GetLatencyStr(result.Last())}[/]");
                                    }
                                    else
                                    {
                                        table.UpdateCell(i, 0, result.First().PingTime.ToString());
                                        table.UpdateCell(i, 2, GetLatencyStr(result.First()));
                                        if (showBypass) table.UpdateCell(i, 4, GetLatencyStr(result.Last()));
                                    }
                                    i++;
                                }

                                lastUpdateTime = dataToShow.TimeStamp;

                                rootTable.UpdateCell(1,0, $"[green]VPN Ip info here[/]");
                                rootTable.UpdateCell(2, 0, $"[blue]Local Ip info here[/]");
                                ctx.Refresh();

                            }

                        }
                        Thread.Sleep(RefreshRateMs);
                    }
                });
        }

        public void Stop()
        {
            AnsiConsole.Clear();
            isActive = false;
        }

        private string GetLatencyStr(PingResult result)
        {
            return result.IsSuccess ? result.PingLatency.ToString() : result.Error;
        }

    }
}

