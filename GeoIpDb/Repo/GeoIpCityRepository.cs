using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GeoIpDb.Repo;
using GeoIpDb.Entities;
using GeoIp;

namespace GeoIpDb.Repo
{
    public class GeoIpCityRepository:RepositoryBase
    {

        string geoIpCityFields = "CityIPID, ContinentID, CountryID, RegionName, CityName, IPRangeStart, IPRangeEnd";

        public GeoIpCityRepository(string connectionString) : base(connectionString)
        {

        }

        public GeoIpCity? GetById(long id)
        {
            return connection.Query<GeoIpCity, IpRange, GeoIpCity>(@$"SELECT {geoIpCityFields}
                    FROM CityIP where CityIPID = @id",
                     map: (geoIpCity, ipRange) =>
                     {
                         geoIpCity.IpRange = ipRange;
                         return geoIpCity;
                     },
                    param: new { id },
                    splitOn: "IPRangeStart").FirstOrDefault();
        }

        public GeoIpCity? GetByIpAddress(string ip)
        {
            return connection.Query<GeoIpCity,IpRange, GeoIpCity>(@$"SELECT {geoIpCityFields}
                    FROM CityIP where @ip > IPRangeStartHex and @ip < IPRangeEndHex",
                    map: (geoIpCity, ipRange) =>
                    {
                        geoIpCity.IpRange = ipRange; 
                        return geoIpCity;
                    },
                    param: new { ip = GeoIpUtils.IpToHex(ip) },
                    splitOn: "IPRangeStart").FirstOrDefault();
        }

        public List<GeoIpCity> GetPage(long startId, long count)
        {
            return connection.Query<GeoIpCity, IpRange, GeoIpCity>(@$"SELECT {geoIpCityFields}
                    FROM CityIP where CityIPID > @startId order by CityIPID limit @count",
                    map: (geoIpCity, ipRange) =>
                    {
                        geoIpCity.IpRange = ipRange;
                        return geoIpCity;
                    },
                    splitOn: "IPRangeStart",
                    param: new { startId, count }).ToList();
        }

        public long GetCount()
        {
            return connection.QueryFirst<long>(@"SELECT count(1) FROM CityIP");
        }

        public void UpdateHexIp(long cityIpId, IpRange ipRange)
        {
            connection.Execute(@"Update CityIP set IPRangeStartHex = @startIpHex, 
                IPRangeEndHex = @endIpHex where CityIPID = @cityIpId", new { startIpHex= GeoIpUtils.IpToHex(ipRange.IpRangeStart), 
                endIpHex= GeoIpUtils.IpToHex(ipRange.IpRangeEnd), cityIpId });
        }



    }
}
