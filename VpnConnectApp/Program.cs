using NonInvasiveKeyboardHookLibrary;
using System.Runtime.InteropServices;
using VpnConnect.Configuration;
using VpnConnect.Console.Presenter;
using VpnConnect.VpnServices;
using VPNConnect;
using VPNConnect.Configuration;
using VPNConnect.UIHandling;


Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var config = ConfigManager.Get();

var vpnServiceFactory = new VpnServiceFactory();

SelectVpnServicePresenter selectVpnServicePresenter = new SelectVpnServicePresenter(vpnServiceFactory);

selectVpnServicePresenter.OnSelected = (service) =>
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

    Application.Run();
};

selectVpnServicePresenter.Select();

