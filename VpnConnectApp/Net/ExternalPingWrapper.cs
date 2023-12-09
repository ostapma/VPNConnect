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
    internal class ExternalPingWrapper: BasePingWrapper
    {
        public ExternalPingWrapper(string procName, string pingTarget) :base(originalExternalPingProcName, procName, pingTarget)
        {
            
        }
        public PingResult GetPingResult()
        {
            PingResult pingResult = null;

            Process proc = new Process();

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = GetProcName();
            proc.StartInfo.Arguments = pingTarget;
            proc.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    pingResult = JsonSerializer.Deserialize<PingResult>(args.Data);
                }
            };
            if (!proc.Start()) throw new Exception($"Failed to start pinger console app {GetProcName()}");
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            return pingResult;
        }

        const string originalExternalPingProcName = "ExternalPing.exe";

    }
}
