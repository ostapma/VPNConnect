using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Views
{
    internal class IpInfoView
    {
        public void ShowIpFormatError(string ip)
        {
            AnsiConsole.MarkupLine($"[red]Invalid ip format: {ip}[/]");
        }

        public void ShowYorCurrentIpError()
        {
            AnsiConsole.MarkupLine($"[red]Faided to get your public IP[/]");
        }

        public void ShowYorCurrentIp(string ip)
        {
            AnsiConsole.MarkupLine($"Your public IP address: [green]{ip}[/]");
        }

        public void ShowLocationIpInfo(string ip, string country, string city)
        {
            AnsiConsole.MarkupLine($"Geoinfo for IP address: [green]{ip}[/]");
            AnsiConsole.MarkupLine($"Country: [green]{country}[/]");
            AnsiConsole.MarkupLine($"City: [green]{city}[/]");
        }

        public void ShowLocationInfoNotFound(string ip)
        {
            AnsiConsole.MarkupLine($"[red]Geoinfo for IP [green]{ip}[/] is not found[/]");
        }

        public void ShowOwnerInfoNotFound(string ip)
        {
            AnsiConsole.MarkupLine($"[red]ASN info for IP [green]{ip}[/] is not found[/]");
        }

        public void ShowOwnerIpInfo(string ip, string owner)
        {
            AnsiConsole.MarkupLine($"ASN info for IP address: [green]{ip}[/]");
            AnsiConsole.MarkupLine($"Owner: [green]{owner}[/]");
        }
    }
}
