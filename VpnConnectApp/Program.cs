using Autofac.Core;
using NonInvasiveKeyboardHookLibrary;
using System.CommandLine;
using System.Runtime.InteropServices;
using VpnConnect.Configuration;
using VpnConnect.Console.Presenters;
using VpnConnect.VpnServices;
using VPNConnect;
using VPNConnect.Configuration;
using VPNConnect.UIHandling;


Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var config = ConfigManager.Get();

var vpnServiceFactory = new VpnServiceFactory();

VpnService service= null;

SelectVpnServicePresenter selectVpnServicePresenter = new SelectVpnServicePresenter(vpnServiceFactory);

var selectVpnOption = new Option<string>("--vpnservice", "Select vpn service")
    .FromAmong(vpnServiceFactory.GetList().Select(s => s.Name.ToLower()).ToArray());
selectVpnOption.AddAlias("-v");
selectVpnOption.Arity = ArgumentArity.ZeroOrOne;


var rootCommand = new RootCommand();
rootCommand.AddOption(selectVpnOption);
rootCommand.SetHandler(selectVpnOptionValue =>
{
    if(selectVpnOptionValue!=null)
    {
        service = vpnServiceFactory.Get(selectVpnOptionValue);
        StartSearch(service);
    }
}, selectVpnOption);

rootCommand.Invoke(args);

if (service == null)
{
    selectVpnServicePresenter.OnSelected += (service) =>
    {
        StartSearch(service);
    };
    selectVpnServicePresenter.Select();
}


void StartSearch(VpnService service)
{
    selectVpnServicePresenter.ShowSelected(service);
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

    Application.Run();
}



