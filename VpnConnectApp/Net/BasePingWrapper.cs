using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;

namespace VpnConnect.Net
{
    internal abstract class BasePingWrapper
    {
        public BasePingWrapper(string procName, string procNameSuffix, string pingTarget)
        {
            this.procName = procName;
            this.procNameSuffix = procNameSuffix;
            this.pingTarget = pingTarget;
            CreateProcCopy();
        }

        private void CreateProcCopy()
        {
            //if (!File.Exists(GetProcName()))
            {
                File.Copy(procName, GetProcName(),true);
            }
        }

        public string GetProcName()
        {
            return $"{Path.GetFileNameWithoutExtension(procName)}{procNameSuffix}{Path.GetExtension(procName)}";
        }

        private readonly string procName;
        private readonly string procNameSuffix;
        protected readonly string pingTarget;
    }
}
