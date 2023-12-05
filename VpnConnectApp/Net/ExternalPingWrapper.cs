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
    internal class ExternalPingWrapper
    {
        public ExternalPingWrapper(string procNameSuffix, string pingTarget)
        {
            this.procNameSuffix = procNameSuffix;
            this.pingTarget = pingTarget;
            CreateProcCopy();
        }

        private void CreateProcCopy()
        {
            if (!File.Exists(GetProcName()))
            {
                File.Copy(originalExternalPingProcName, GetProcName());
            }
        }

        public string GetProcName()
        {
            return $"{Path.GetFileNameWithoutExtension(originalExternalPingProcName)}{procNameSuffix}{Path.GetExtension(originalExternalPingProcName)}";
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
            proc.Start();
            proc.BeginOutputReadLine();
            proc.WaitForExit();
            return pingResult;
        }

        const string originalExternalPingProcName = "ExternalPing.exe";
        private readonly string procNameSuffix;
        private readonly string pingTarget;
    }
}
