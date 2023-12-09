using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VPNConnect.Net;

namespace VpnConnect.Net
{
    internal class TcpPingWrapper: BasePingWrapper
    {
        const string PingCommand = "p";
        const string StopCommand = "s";
        const int TcpPingExitTimeoutMs = 10000;

        Process proc;
        StreamWriter consoleWriter;

        string pingResultStr;

        public TcpPingWrapper(string procName, string pingTarget, int tcpPort) : base(originalExternalPingProcName, procName,  pingTarget)
        {
            this.tcpPort = tcpPort;
        }

        public void StartPinging()
        {
            proc = new Process();

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.FileName = GetProcName();
            proc.StartInfo.Arguments = $"{pingTarget} {tcpPort}";

            proc.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    pingResultStr = args.Data;
                }
            };
            if (!proc.Start()) throw new Exception($"Failed to start pinger console app {GetProcName()}");

            consoleWriter = proc.StandardInput;
            proc.BeginOutputReadLine();
        }

        public PingResult GetPingResult()
        {
            var oldPingResult = pingResultStr;

            consoleWriter.WriteLine(PingCommand);
            consoleWriter.Flush();

            while (pingResultStr == oldPingResult)
            {
                Thread.Sleep(100);
            }
            return JsonSerializer.Deserialize<PingResult>(pingResultStr);
        }

        public void StopPinging() 
        {
            consoleWriter.WriteLine(StopCommand);
            consoleWriter.Flush();

            if(!proc.WaitForExit(TcpPingExitTimeoutMs))
                throw new Exception($"Failed to stop pinger console app {GetProcName()}");
            consoleWriter.Close();
        }

        const string originalExternalPingProcName = "tcpping.exe";
        private readonly int tcpPort;
    }
}
