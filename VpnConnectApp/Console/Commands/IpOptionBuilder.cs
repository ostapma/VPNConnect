using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Commands
{
    internal class IpOptionBuilder
    {
        protected string Name { get; } = "--ip";
        protected string Alias { get; } = "-ip";
        protected string Description { get; } = "IP Address";

        public Option<string> Build()
        {
            var option = new Option<string>(Name, Description);
            option.AddAlias(Alias);
            option.Arity = ArgumentArity.ZeroOrOne;

            return option;
        }
    }
}
