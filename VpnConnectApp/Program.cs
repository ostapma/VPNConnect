﻿using NonInvasiveKeyboardHookLibrary;
using System.Runtime.InteropServices;
using VPNConnect;
using VPNConnect.Configuration;
using VPNConnect.UIHandling;

[DllImport("kernel32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool AllocConsole();

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

AllocConsole();

var config = ConfigManager.Get();

VpnServiceType? vpnService=null;


while (vpnService==null)
{
    Console.WriteLine("Type w for windscribe or h for hideme");
    string vpnServiceInput = Console.ReadLine();
    vpnService =  vpnServiceInput.ToLower() switch
    {
        "w" =>  VpnServiceType.Windscribe,
        "h" =>  VpnServiceType.Hideme,
        _ => null,
    };
    if (vpnService==null) Console.WriteLine($"{vpnServiceInput} is not a valid value");

}

Console.WriteLine($"Put your mouse cursor on {vpnService} VPN client's connection button center");
Console.WriteLine($"Press {config.ConsoleSettings.StartHotKey} to start {vpnService} VPN searching");

IVpnUiHandler vpnUiHandler = UiHandlerFactory.GetHandler(vpnService.Value);

VpnSearcher searcher = new(vpnUiHandler, config.ConsoleSettings, config.VpnUiHandlingSettings, config.NetAnanlyzeSettings,
    config.GeoIpDbSettings, config.ExternalIpServiceLink);

try
{
    searcher.StartHotkey();
}

catch (Exception e)
{
    Log.Error($"Something went wrong on start: {e}");
}

Application.ApplicationExit += (ev, t) =>
{
    searcher.StopHotkey();
};

Application.Run();
