namespace VpnConnect.Configuration
{
    public class VpnSearchSettings
    {
        public ConsoleSettings ConsoleSettings { get; init; }
        public VpnUiHandlingSettings VpnUiHandlingSettings { get; init; }

        public GeoIpDbSettings GeoIpDbSettings { get; init; }

        public NetAnanlyzeSettings NetAnanlyzeSettings { get; init; }

        public TargetApplicationSettings TargetApplicationSettings { get; init; }

        public string ExternalIpServiceLink { get; init; }

    }
}