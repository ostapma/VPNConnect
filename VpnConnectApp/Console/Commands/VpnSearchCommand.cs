using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Configuration;
using VpnConnect.Console.Presenters;
using VpnConnect.VpnServices;
using VPNConnect;
using VPNConnect.Configuration;

namespace VpnConnect.Console.Commands
{
    internal class VpnSearchCommand
    {
        protected string Name { get; } = "vpnsearch";

        protected string Description { get; } = "Search vpn by criteria in settings";

        public void Execute(string commandline) {
            var searchCommand = new Command(Name, Description);
            var vpnServiceFactory = new VpnServiceFactory();
            SelectVpnServiceOption selectVpnServiceOption = new SelectVpnServiceOption(vpnServiceFactory.GetList().Select(s => s.Name.ToLower()).ToList());
            Option<string> selOption = selectVpnServiceOption.GetOption();
            SelectVpnServicePresenter selectVpnServicePresenter = new SelectVpnServicePresenter(vpnServiceFactory);

            RootCommand rootCommand = new RootCommand();
            rootCommand.Add(searchCommand);
            searchCommand.SetHandler((selectVpnOptionValue) =>
            {
                if (selectVpnOptionValue != null)
                {
                    selectVpnServicePresenter.SelectVpn(selectVpnOptionValue.ToString());
                }
                else selectVpnServicePresenter.ShowSelector();
            }, selOption);
            rootCommand.Invoke(commandline);

            selectVpnServicePresenter.OnSelected += (service) =>
            {
                VpnSearchSettings settings = ConfigManager.Get().Settings();

                VpnSearcher searcher = new(service, settings);
                VpnSearchPresenter searchPresenter = new VpnSearchPresenter(service, settings, searcher);
                searchPresenter.ShowStartPrompt();

                try
                {
                    searchPresenter.SubscribeKeys();
                }

                catch (Exception e)
                {
                    Log.Error($"Something went wrong on start: {e}");
                }

                Application.ApplicationExit += (ev, t) =>
                {
                    searchPresenter.StopSearch();
                    searchPresenter.UnsubscribeKeys();
                };

            };
        }
    }
}
