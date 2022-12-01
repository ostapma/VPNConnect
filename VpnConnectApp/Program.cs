using Autofac.Core;
using NonInvasiveKeyboardHookLibrary;
using System.CommandLine;
using System.Runtime.InteropServices;
using System.Security;
using VpnConnect.Configuration;
using VpnConnect.Console.Commands;
using VpnConnect.Console.Presenters;
using VpnConnect.VpnServices;
using VPNConnect;
using VPNConnect.Configuration;
using VPNConnect.UIHandling;


Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var config = ConfigManager.Get();

var vpnServiceFactory = new VpnServiceFactory();

SelectVpnServicePresenter selectVpnServicePresenter = new SelectVpnServicePresenter(vpnServiceFactory);

SelectVpnServiceOption selectVpnServiceOption = new SelectVpnServiceOption(vpnServiceFactory.GetList().Select(s => s.Name.ToLower()).ToList());

selectVpnServicePresenter.OnSelected += (service) =>
{
    VpnSearchSettings settings = ConfigManager.Get().Settings();

    Console.WriteLine($"Put your mouse cursor on {service.Name} VPN client's connection button center");
    Console.WriteLine($"Press {settings.ConsoleSettings.StartHotKey} to start {service.Name} VPN searching");

    VpnSearcher searcher = new(service, settings);

    try
    {
        searcher.Start();
    }

    catch (Exception e)
    {
        Log.Error($"Something went wrong on start: {e}");
    }

    Application.ApplicationExit += (ev, t) =>
    {
        searcher.Stop();
    };

};

string serviceTypeStr = selectVpnServiceOption.GetValue(string.Join(" ", args));
if (serviceTypeStr != null)
{
    selectVpnServicePresenter.SelectVpn(serviceTypeStr);
}
else selectVpnServicePresenter.ShowSelector();

Application.Run();

