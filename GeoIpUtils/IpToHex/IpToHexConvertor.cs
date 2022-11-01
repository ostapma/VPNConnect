using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using VPNConnect.Net;
using GeoIp.Entities;
using GeoIp.Repo;

namespace GeoIpUtils.IpToHex
{

    //            connection.Execute("PRAGMA synchronous=OFF");
    internal class IpToHexConvertor
    {
        string conn = "Data Source=C:\\Work\\Winsconnect\\Winsconnect\\geoip.db";
        int pageSize = 1000;

        public void ConvertForCity()
        {
            GeoIpRepository geoIpRepository = new GeoIpRepository(conn);

            var page = geoIpRepository.GetPage(144000, pageSize);

            do
            {
                if (page.Count() > 0)
                {
                    Log.Information($"Processing {pageSize} records starting from {page.First().CityIpId}");

                    foreach (var cityIp in page)
                    {
                        geoIpRepository.UpdateHexIp(cityIp.CityIpId, cityIp.IpRangeStart, cityIp.IpRangeEnd);
                    }
                    page = geoIpRepository.GetPage(page.Last().CityIpId, pageSize);
                }
            }
            while (page.Count() >= pageSize);
            }
        }
    }

