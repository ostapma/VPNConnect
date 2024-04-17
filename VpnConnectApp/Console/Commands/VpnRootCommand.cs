using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Commands
{
    internal class VpnRootCommand
    {
        RootCommand rootCommand;

        public void Register()
        {
            rootCommand = new RootCommand();
            rootCommand.AddCommand(new VpnSearchCommandBuilder().Build());
            rootCommand.AddCommand(new IpInfoCommandBuilder().Build());
            rootCommand.AddCommand(new NetmonCommandBuilder().Build());
            rootCommand.AddCommand(new IpMarkCommand().Build());
        }

        public void Execute(string commandLine) {
            rootCommand.Invoke(commandLine);
        }
    }
}
