using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.VpnServices;

namespace VPNConnect.UIHandling
{
    internal class UiHandlerFactory
    {
        public static IVpnUiHandler GetHandler(VpnServiceType vpnService)
        {
            return vpnService switch
            {
                VpnServiceType.Windscribe => new WindscribeVpnService(),
                VpnServiceType.Hideme => new HidemeVpnService(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
