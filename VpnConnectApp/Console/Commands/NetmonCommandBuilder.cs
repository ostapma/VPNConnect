using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Presenters;
using VPNConnect;
using VPNConnect.Configuration;

namespace VpnConnect.Console.Commands
{
    internal class NetmonCommandBuilder : BaseCommandBuilder
    {
        public NetmonCommandBuilder()
        {
            Name = "netmon";
            Description = "Monitor internet connection";
        }

        public override Command Build()
        {
            var command = new Command(Name, Description);


            command.SetHandler(() =>
            {
                NetmonPresenter netmonPresenter = new NetmonPresenter(ConfigManager.Get().Settings().NetAnanlyzeSettings.PingTarget);
                netmonPresenter.Monitor();
            });
            return command;
        }
    }
}
