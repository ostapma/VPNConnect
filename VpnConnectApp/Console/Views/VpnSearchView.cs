using Autofac.Core;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Views
{
    internal class VpnSearchView
    {
        internal void ShowGetMyIpError()
        {
            AnsiConsole.MarkupLine($"[red]Unable to get your public IP address[/]");
        }

        internal void ShowMyIp(string myIp)
        {
            AnsiConsole.MarkupLine($"Your public IP address [blue]{myIp}[/]");
        }

        internal void ShowSearchStop(string stopKey)
        {
            AnsiConsole.MarkupLine($"Search stop [blue]{stopKey}[/] pressed");
            AnsiConsole.MarkupLine("Wait for VPN searching to stop");
        }

        internal void ShowStartPrompt(string vpnName, string startKey, string stopKey)
        {
            AnsiConsole.MarkupLine($"Put your mouse cursor on [blue]{vpnName}[/] VPN client's Connect button center");
            AnsiConsole.MarkupLine($"Press [blue]{startKey}[/] to start [blue]{vpnName}[/] VPN searching");
            AnsiConsole.MarkupLine($"Press [blue]{stopKey}[/] to stop [blue]{vpnName}[/] VPN searching");
        }
    }
}
