using VPNConnect.Net;

namespace VpnConnect.Net
{
    internal interface IPinger
    {
        PingResult GetPingResult();
        void StartPinging();
        void StopPinging();
    }
}