
using GeoIp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using GeoIpDb.Entities;

namespace GeoIpDb.Repo
{
    public class GeoIpAsnRepository:RepositoryBase
    {
        string geoIpAsnIpFields = "ASNIP.ASNIPID, IsBlacklisted, ASN.ASNID, ASN.ASNName Name, IPRangeStart, IPRangeEnd";

        public GeoIpAsnRepository(string connectionString) :base(connectionString)
        {

        }

        public GeoIpAsnIp? GetByIpAddress(string ip)
        {
            return connection.Query<GeoIpAsnIp, GeoIpAsn,IpRange,  GeoIpAsnIp>(@$"SELECT {geoIpAsnIpFields}  FROM ASNIP
                    join ASN on ASNIP.ASNID = ASN.ASNID where @ip > IPRangeStartHex and @ip < IPRangeEndHex",
                     map: (geoIpAsnIp, geoIpAsn, ipRange) =>
                     {
                         geoIpAsnIp.IpRange = ipRange;
                         geoIpAsnIp.Asn = geoIpAsn;
                         return geoIpAsnIp;
                     },
                    splitOn: "ASNID, IPRangeStart",
                   
                    param: new { ip = GeoIpUtils.IpToHex(ip) }).FirstOrDefault();
        }

        public List <GeoIpAsnIp> GetPage(long startId, long count)
        {
            return connection.Query<GeoIpAsnIp, GeoIpAsn, IpRange, GeoIpAsnIp>(@$"SELECT {geoIpAsnIpFields}  FROM ASNIP
                    join ASN on ASNIP.ASNID = ASN.ASNID WHERE ASNIPID > @startId order by ASNIPID limit @count",
                     map: (geoIpAsnIp, geoIpAsn, ipRange) =>
                     {
                         geoIpAsnIp.IpRange = ipRange;
                         geoIpAsnIp.Asn = geoIpAsn;
                         return geoIpAsnIp;
                     },
                    splitOn: "ASNID, IPRangeStart",

                    param: new { startId, count }).ToList();
        }

        public long GetCount()
        {
            return connection.QueryFirst<long>(@"SELECT count(1) FROM ASNIP");
        }

        public void UpdateHexIp(long asnIpId, IpRange ipRange)
        {
            connection.Execute(@"Update ASNIP set IPRangeStartHex = @startIpHex, 
                IPRangeEndHex = @endIpHex where ASNIPID = @asnIpId", new
            {
                startIpHex = GeoIpUtils.IpToHex(ipRange.IpRangeStart),
                endIpHex = GeoIpUtils.IpToHex(ipRange.IpRangeEnd),
                asnIpId
            });
        }

    }
}
