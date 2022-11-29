using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.VpnServices;

namespace VpnConnect.Console.View
{
    internal class SelectVpnServiceView
    {
        public void ShowSelect(List<string> services)
        {
            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select VPN service you are using")
                .AddChoices(services));
            OnSelected(selected);
        }

        public Action<string> OnSelected;
    }
}
