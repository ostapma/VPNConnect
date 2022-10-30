using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public ConsoleSettings ConsoleSettings {

            get
            {
                return config.GetSection("consoleSettings").Get<ConsoleSettings>();
            }
        }

        public VpnUiHandlingSettings VpnUiHandlingSettings
        {
            get
            {
                return config.GetSection("vpnUiHandlingSettings").Get<VpnUiHandlingSettings>();
            }
        }

        public NetAnanlyzeSettings NetAnanlyzeSettings
        {
            get
            {
                return config.GetSection("netAnanlyzeSettings").Get<NetAnanlyzeSettings>();
            }
        }

        public GeoIpDbSettings GeoIpDbSettings
        {
            get
            {
                return config.GetSection("geoIpDbSettings").Get<GeoIpDbSettings>();
            }
        }

        public string ExternalIpServiceLink
        {
            get
            {
                return config.GetSection("ExternalIpServiceLink").Get<string>();
            }
        }
    }

}
