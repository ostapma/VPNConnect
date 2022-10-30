using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNConnect.UIHandling
{
    internal class WindscribeUiHandler : IVpnUiHandler
    {
        MouseClicker clicker = new MouseClicker();
        public void PressConnect()
        {
            clicker.LeftClick();
        }

        public void PressDisconnect()
        {
            clicker.LeftClick();
        }
    }
}
