using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Data;
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
            Option<bool> bypassOption = new Option<bool>(new string[] { "--bypass", "-b" }, "bypass vpn to ping (needs setup, check docs for more info)");
            bypassOption.Arity = ArgumentArity.Zero;
            command.AddOption(bypassOption);

            command.SetHandler((bp) =>
            {
                NetmonPresenter netmonPresenter = new NetmonPresenter(ConfigManager.Get().Settings().NetAnanlyzeSettings.PingTarget, bp);
                netmonPresenter.Monitor();
            }, bypassOption);
            return command;
        }
    }
}
