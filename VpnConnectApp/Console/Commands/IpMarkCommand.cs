using GeoIpDb.Repo;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Configuration;

namespace VpnConnect.Console.Commands
{
    internal class IpMarkCommand : BaseCommandBuilder
    {
        public IpMarkCommand()
        {
            Name = "ipmark";
            Description = "Mark IP as good/blacklisted or add some comments";
        }

        public override Command Build()
        {
            var command = new Command(Name, Description);
            Option<bool> blacklistOption = new Option<bool>(new string[] { "--blacklisted", "-b" });
            blacklistOption.Arity = ArgumentArity.Zero;
            command.AddOption(blacklistOption);
            Option<bool> goodOption = new Option<bool>(new string[] { "--good", "-g" });
            goodOption.Arity = ArgumentArity.Zero;
            command.AddOption(goodOption);
            Option<string?> commentOption = new Option<string?>(new string[] { "--comment", "-c" });
            command.AddOption(commentOption);
            command.SetHandler((blo, go, co) =>
            {
                var ipService = new VPNConnect.Net.ExternalIpServiceProvider(ConfigManager.Get().Settings().ExternalIpServiceLink);
                var ip = ipService.GetMyIp();
                if (ip != null && ip.IsSuccess)
                {
                    IPAddress? ipParsed;
                    if (IPAddress.TryParse(ip.IpAddress, out ipParsed))
                    {
                        var knownIpPoolRepository = new GeoIpDb.Repo.KnownIpPoolRepository(ConfigManager.Get().Settings().GeoIpDbSettings.ConnectionString);
                        knownIpPoolRepository.Add(new GeoIpDb.Entities.KnownIpPool
                        {
                            ApplicationId = ConfigManager.Get().Settings().TargetApplicationSettings.ApplicationId,
                            Comments = co,
                            IsBlacklisted = blo,
                            IsGood = go,
                            IpRange = new GeoIpDb.Entities.IpRange { IpRangeStart = ipParsed.ToString(), IpRangeEnd = ipParsed.ToString() }

                        });
                        AnsiConsole.MarkupLine($"IP address [green]{ipParsed}[/] has been marked in our database");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Incorrect IP address[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Failed to get IP address from ip service provider[/]");
                }
            }, blacklistOption, goodOption, commentOption);
            return command;
        }
    }
}
