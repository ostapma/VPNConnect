using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Commands;
using VpnConnect.Console.Views;

namespace VpnConnect.Console.Presenters
{
    internal class CliInputPresenter
    {
        CliInputView view;
        public CliInputPresenter()
        {
            view = new CliInputView();
        }

        public string InputCommand()
        {
            return view.GetInput();
        }
    }
}
