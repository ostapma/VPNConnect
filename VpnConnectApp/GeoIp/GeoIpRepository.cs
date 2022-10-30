using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.GeoIp.Entities;

namespace VPNConnect.GeoIp
{
    public class GeoIpRepository
    {
        SqliteConnection connection;

        public GeoIpRepository(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
        }

        public GeoIpCity GetById(long id)
        {
            return connection.QueryFirst<GeoIpCity>(@"SELECT CityIPID, IPRangeStart, IPRangeEnd, 
                    ContinentID, CountryID, RegionName, CityName
                    FROM CityIP where CityIPID = @id", new { id });
        }

        public GeoIpCity GetByIpAddress(long ipHex)
        {
            return connection.QueryFirst<GeoIpCity>(@"SELECT CityIPID, IPRangeStart, IPRangeEnd, 
                    ContinentID, CountryID, RegionName, CityName
                    FROM CityIP where @ip > IPRangeStartHex and @ip < IPRangeEndHex", new { ip = ipHex });
        }

        public IEnumerable<GeoIpCity> GetPage(long startId, long count)
        {
            return connection.Query<GeoIpCity>(@"SELECT CityIPID, IPRangeStart, IPRangeEnd, 
                    ContinentID, CountryID, RegionName, CityName
                    FROM CityIP where CityIPID > @startId order by CityIPID limit @count", new { startId, count });
        }

        public long GetCount()
        {
            return connection.QueryFirst<long>(@"SELECT count(1) FROM CityIP");
        }

        public void UpdateHexIp(long cityIpId, long startIpHex, long endIpHex)
        {
            connection.Execute(@"Update CityIP set IPRangeStartHex = @startIpHex, 
                IPRangeEndHex = @endIpHex where CityIPID = @cityIpId", new { startIpHex, endIpHex, cityIpId });
        }

        //public GeoIpCity FindCityByIp(string ipAddress)
        //{
        //    using var connection = new SqliteConnection(connectionString);

        //    connection.CreateFunction(
        //        "ipToInt",
        //        (string ipString)
        //             => IpToInt(ipString));

        //    return connection.QueryFirst<GeoIpCity>(@"SELECT CityIPID, IPRangeStart, IPRangeEnd, 
        //            ContinentID, CountryID
        //            FROM CityIP where ipToInt(IPRangeStart)<=@ip and ipToInt(IPRangeEnd)>=@ip;", new { ip = IpToInt(ipAddress) });
        //}
    }
}
