using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.Console.Commands
{

    public abstract class BaseCommandBuilder
    {
        protected string Name { get; init; }

        protected string Description { get; init; }

        public abstract Command Build();
    }
}
