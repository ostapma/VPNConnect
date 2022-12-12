using VPNConnect.VpnClientHandling;

namespace VpnConnect.VpnServices
{
    internal class HidemeVpnService : VpnService
    {
        public HidemeVpnService() : base("Hideme")
        {

        }


        (int x, int y) adPopupAjustment = new(-100, 70);

        MouseClicker clicker = new MouseClicker();
        public override void PressConnect()
        {
            clicker.LeftClick();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var currentPosition = clicker.GetPosition();
            clicker.SetPosition(currentPosition.x + adPopupAjustment.x, currentPosition.y + adPopupAjustment.y);
            clicker.LeftClick();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            clicker.SetPosition(currentPosition.x, currentPosition.y);
        }

        public override void PressDisconnect()
        {
            int waitHidemeUiLagSec = 5;
            Thread.Sleep(TimeSpan.FromSeconds(waitHidemeUiLagSec));
            clicker.LeftClick();
        }
    }
}
