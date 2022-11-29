using VPNConnect.UIHandling;

namespace VpnConnect.VpnServices
{
    internal abstract class VpnService: IVpnUiHandler
    {
        public VpnService(string name)
        {
            Name = name;
        }
        public string Name { get; init; }

        public abstract void PressConnect();
        public abstract void PressDisconnect();
    }
}
