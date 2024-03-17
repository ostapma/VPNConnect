using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine.IO;

namespace VpnConnect.Console.Views
{
    internal class CliInputView
    {


        List<string> commandStack = new List<string>();
        public string GetInput()
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write(">> ");
            System.Console.ForegroundColor = ConsoleColor.White;
            string prompt = System.Console.ReadLine(); 
            commandStack.Add(prompt);
            return prompt;
        }
    }
}
