using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.VpnServices;

namespace VpnConnect.Console.Views
{
    internal class SelectVpnServiceView
    {
        public void AskSelect(List<string> services)
        {
            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select VPN service you are using")
                .AddChoices(services));
            OnSelected(selected);
        }

        public void ShowSelected(string selectedService)
        {
            AnsiConsole.MarkupLine($"Selected vpn service: [blue]{selectedService}[/]");
        }

        public event Action<string> OnSelected;
    }
}
