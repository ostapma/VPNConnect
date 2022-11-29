using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnect.VpnServices
{
    internal class VpnServiceFactory
    {
        List<VpnService> services = new List<VpnService>() { 
            new VpnServices.HidemeVpnService(), 
            new VpnServices.WindscribeVpnService() };

        public List <VpnService> GetList() { 
            return services; 
        }
        public VpnService? Get(string name)
        {
            return services.Find(s=>s.Name.ToLower() == name.ToLower());
        }
        
    }
}
