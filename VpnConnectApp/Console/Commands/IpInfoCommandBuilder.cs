using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Console.Presenters;
using VPNConnect.Configuration;

namespace VpnConnect.Console.Commands
{
    internal class IpInfoCommandBuilder : BaseCommandBuilder
    {
        public IpInfoCommandBuilder()
        {
            Name  = "ipinfo";
            Description = "Get IP address info for current public IP or for selected IP in option";
        }

        public override Command Build()
        {
            var command = new Command(Name, Description);
            Option<string> option
                = new IpOptionBuilder().Build();
            command.AddOption(option);
            command.SetHandler((ipOptionValue) =>
            {
                IpInfoPresenter ipInfoPresenter = new IpInfoPresenter(
                    new VPNConnect.Net.ExternalIpServiceProvider(ConfigManager.Get().Settings().ExternalIpServiceLink),
                    new GeoIpDb.Repo.GeoIpCityRepository(ConfigManager.Get().Settings().GeoIpDbSettings.ConnectionString),
                    new GeoIpDb.Repo.GeoIpAsnRepository(ConfigManager.Get().Settings().GeoIpDbSettings.ConnectionString),
                    new GeoIpDb.Repo.KnownIpPoolRepository(ConfigManager.Get().Settings().GeoIpDbSettings.ConnectionString));
                ipInfoPresenter.IpInfo(ipOptionValue);
            },option);
            return command;
        }
    }
}
