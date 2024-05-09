using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using VPNConnect.Net;
using GeoIpDb.Repo;
using System.Net;

namespace GeoIpUtils.IpToHex
{

    //            connection.Execute("PRAGMA synchronous=OFF");
    internal class IpToHexConvertor
    {
        public IpToHexConvertor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        int pageSize = 10000;
        private readonly string connectionString;

        public void ConvertForCity()
        {
            var geoIpRepository = new GeoIpCityRepository(connectionString);
            geoIpRepository.SetSynchronousModeOff();
            var page = geoIpRepository.GetPage(0, pageSize);

            do
            {
                if (page.Count() > 0)
                {
                    Log.Information($"Processing {pageSize} records starting from {page.First().CityIpId}");

                    foreach (var cityIp in page)
                    {
                        if(IPAddress.Parse(cityIp.IpRange.IpRangeStart).AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork)
                            geoIpRepository.UpdateHexIp(cityIp.CityIpId, cityIp.IpRange);
                    }
                    page = geoIpRepository.GetPage(page.Last().CityIpId, pageSize);
                }
            }
            while (page.Count() >= pageSize);
            }
        public void ConvertForAsn()
        {
            var geoIpRepository = new GeoIpAsnRepository(connectionString);
            geoIpRepository.SetSynchronousModeOff();

            var page = geoIpRepository.GetPage(0, pageSize);

            do
            {
                if (page.Count() > 0)
                {
                    Log.Information($"Processing {pageSize} records starting from {page.First().AsnIpId}");

                    foreach (var asnIp in page)
                    {
                        if (IPAddress.Parse(asnIp.IpRange.IpRangeStart).AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            geoIpRepository.UpdateHexIp(asnIp.AsnIpId, asnIp.IpRange);
                    }
                    page = geoIpRepository.GetPage(page.Last().AsnIpId, pageSize);
                }
            }
            while (page.Count() >= pageSize);
        }
    }


    }

