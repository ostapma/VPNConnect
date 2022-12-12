using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VPNConnect.VpnClientHandling
{
    internal class MouseClicker
    {

        const int LeftDown = 0x00000002;
        const int LeftUp = 0x00000004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        public void LeftClick()
        {
            (int x, int y) = GetPosition();
            
            mouse_event(LeftDown | LeftUp, x, y, 0, 0);
        }

        public (int x,int y) GetPosition()
        {
            return new(Cursor.Position.X, Cursor.Position.Y);
        }

        public void SetPosition(int x, int y)
        {
            Cursor.Position = new(x, y);
        }

    }
}
