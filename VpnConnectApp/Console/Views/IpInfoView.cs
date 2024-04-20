using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        internal void ShowKnownIpInfo(DateTime dateAdded, string comments, bool isBlacklisted, bool isGood)
        {
            AnsiConsole.MarkupLine($"This IP marked in our DB as:");
            AnsiConsole.MarkupLine($"  Added [green]{dateAdded}[/]");
            if(isBlacklisted) AnsiConsole.MarkupLine("  [green]Blacklisted[/]");
            if (isGood) AnsiConsole.MarkupLine("  [green]Good[/]");
            if(!string.IsNullOrEmpty(comments)) AnsiConsole.MarkupLine($"  [green]{comments}[/]");
        }
    }
}
