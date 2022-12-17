using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Views
{
    internal class CliInputView
    {
        public string GetInput()
        {
            return AnsiConsole.Ask<string>("[green]>>[/]");
        }
    }
}
