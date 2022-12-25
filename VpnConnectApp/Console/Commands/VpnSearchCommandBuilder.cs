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
    internal class VpnSearchCommandBuilder: BaseCommandBuilder
    {
        public VpnSearchCommandBuilder()
        {
            Name  = "vpnsearch";
            Description = "Search vpn by criteria in settings";
        }

        public override Command Build() {
            var searchCommand = new Command(Name,Description);
            var vpnServiceFactory = new VpnServiceFactory();
            SelectVpnServiceOptionBuilder selectVpnServiceOption = new SelectVpnServiceOptionBuilder(vpnServiceFactory.GetList().Select(s => s.Name.ToLower()).ToList());
            Option<string> selOption = selectVpnServiceOption.Build();
            SelectVpnServicePresenter selectVpnServicePresenter = new SelectVpnServicePresenter(vpnServiceFactory);
            searchCommand.AddOption(selOption);
            
            searchCommand.SetHandler((selectVpnOptionValue) =>
            {
                if (selectVpnOptionValue != null)
                {
                    selectVpnServicePresenter.SelectVpn(selectVpnOptionValue.ToString());
                }
                else selectVpnServicePresenter.ShowSelector();
            }, selOption);


            selectVpnServicePresenter.OnSelected += (service) =>
            {
                VpnSearchSettings settings = ConfigManager.Get().Settings();

                VpnSearcher searcher = new(service, settings);
                Action onStopped = () =>
                {
                    Application.Exit();
                };
                VpnSearchPresenter searchPresenter = new VpnSearchPresenter(service, settings, searcher, onStopped);
                searchPresenter.Start();


                Application.Run();
            };

            return searchCommand;
        }
    }
}
