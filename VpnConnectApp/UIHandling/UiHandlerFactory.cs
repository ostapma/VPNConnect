using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNConnect.UIHandling
{
    internal class UiHandlerFactory
    {
        public static IVpnUiHandler GetHandler(VpnServiceType vpnService)
        {
            return vpnService switch
            {
                VpnServiceType.Windscribe => new WindscribeUiHandler(),
                VpnServiceType.Hideme => new HidemeUiHandler(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
