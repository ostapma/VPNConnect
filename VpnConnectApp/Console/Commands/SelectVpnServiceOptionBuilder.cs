using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Commands
{
    internal class SelectVpnServiceOptionBuilder
    {
        private readonly List<string> serviceList;

        protected string Name { get; } = "--vpnservice";
        protected string Alias { get; } = "-v";
        protected string Description { get; } = "Select vpn service";

        public SelectVpnServiceOptionBuilder(List<string> serviceList)
        {

            this.serviceList = serviceList;
        }

        public Option<string> Build()
        {
            var selectVpnOption = new Option<string>(Name, Description)
               .FromAmong(serviceList.ToArray());
            selectVpnOption.AddAlias(Alias);
            selectVpnOption.Arity = ArgumentArity.ZeroOrOne;
            var rootCommand = new RootCommand();

            rootCommand.AddOption(selectVpnOption);
            rootCommand.SetHandler(val => { }, selectVpnOption);
            return selectVpnOption;
        }

        
    }
}
