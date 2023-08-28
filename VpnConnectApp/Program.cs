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
using VPNConnect.VpnClientHandling;


//[DllImport("kernel32.dll", SetLastError = true)]
//[return: MarshalAs(UnmanagedType.Bool)]
//static extern bool AllocConsole();

//Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

//var config = ConfigManager.Get();


//AllocConsole();

CliInputPresenter cliInputPresenter= new CliInputPresenter();
VpnRootCommand vpnRootCommand= new VpnRootCommand();
vpnRootCommand.Register();
string command;
if (args.Count() > 0)
{
    command = string.Join(" ", args);
    vpnRootCommand.Execute(command);
}
while(true)
{
    command = cliInputPresenter.InputCommand();
    vpnRootCommand.Execute(command);
}




