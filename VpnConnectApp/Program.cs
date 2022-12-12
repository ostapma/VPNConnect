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


Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var config = ConfigManager.Get();

VpnSearchCommand searchCommand= new VpnSearchCommand();
if (args.Count() > 0)
    searchCommand.Execute(string.Join(" ", args));
else searchCommand.Execute("vpnsearch");
Application.Run();

