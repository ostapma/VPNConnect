using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VpnConnect.Configuration;
using VPNConnect.Net;

namespace VPNConnect.Configuration
{


    internal class ConfigManager
    {
        private static ConfigManager? instance = null;
        IConfiguration config;

        private ConfigManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            config = builder.Build();
        }

        public static ConfigManager Get()
        {
            if (instance == null)
            {

                instance = new ConfigManager();
            }
            return instance;

        }

        public VpnSearchSettings Settings()
        {
            return new VpnSearchSettings()
            {
                ConsoleSettings = config.GetSection("consoleSettings").Get<ConsoleSettings>(),
                VpnUiHandlingSettings = config.GetSection("vpnUiHandlingSettings").Get<VpnUiHandlingSettings>(),
                NetAnanlyzeSettings = config.GetSection("netAnanlyzeSettings").Get<NetAnanlyzeSettings>(),
                GeoIpDbSettings = config.GetSection("geoIpDbSettings").Get<GeoIpDbSettings>(),
                ExternalIpServiceLink = config.GetSection("ExternalIpServiceLink").Get<string>(),
                TargetApplicationSettings = config.GetSection("targetApplicationSettings").Get<TargetApplicationSettings>()
            };
        }

       
    }

}
