namespace VPNConnect
{

    public class ConsoleSettings
    {
        public string StartHotKey { get; set; } = "";
        public string StopHotKey { get; set; } = "";
    }

    public class VpnUiHandlingSettings
    {
        public int ConnectTimeoutSec { get; set; } = 0;
    }

    public class NetAnanlyzeSettings
    {
        public string PingTarget { get; set; } = "";
        public int TolerablePacketLoss { get; set; } = 0;

        public int TolerableLatencySec { get; set; } = 0;

        public List<string> BlacklistCountries { get; set; } = new List<string>();
        public int PingHops { get; set; } = 0;
    }

    public class GeoIpDbSettings
    {
        public string ConnectionString { get; set; } = "";
    }
}