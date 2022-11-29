using VPNConnect.UIHandling;

namespace VpnConnect.VpnServices
{
    internal class WindscribeVpnService : VpnService
    {
        public WindscribeVpnService() : base("Windscribe")
        {

        }

        MouseClicker clicker = new MouseClicker();
        public override void PressConnect()
        {
            clicker.LeftClick();
        }

        public override void PressDisconnect()
        {
            clicker.LeftClick();
        }
    }
}
