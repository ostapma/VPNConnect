using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNConnect.UIHandling
{
    internal class HidemeUiHandler : IVpnUiHandler
    {
        (int x, int y) adPopupAjustment = new(-100, 70);

        MouseClicker clicker = new MouseClicker();
        public void PressConnect()
        {
            clicker.LeftClick();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var currentPosition = clicker.GetPosition();
            clicker.SetPosition(currentPosition.x+ adPopupAjustment.x, currentPosition.y+ adPopupAjustment.y);
            clicker.LeftClick();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            clicker.SetPosition(currentPosition.x, currentPosition.y);
        }

        public void PressDisconnect()
        {
            int waitHidemeUiLagSec = 5;
            Thread.Sleep(TimeSpan.FromSeconds(waitHidemeUiLagSec));
            clicker.LeftClick();
        }
    }
}
