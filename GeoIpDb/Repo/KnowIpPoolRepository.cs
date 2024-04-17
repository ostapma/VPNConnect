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
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace GeoIpDb.Repo
{
    public class KnownIpPoolRepository : RepositoryBase
    {

        string knownIpPoolFields = "KnownIPPoolID, Comments, DateAdded, IsBlacklisted, IsGood, ApplicationID, IPRangeStart, IPRangeEnd";

        public KnownIpPoolRepository(string connectionString) : base(connectionString)
        {

        }

        public KnownIpPool? GetById(long id)
        {
            return connection.Query<KnownIpPool, IpRange, KnownIpPool>(@$"SELECT {knownIpPoolFields}
                    FROM KnownIPPool where KnownIPPoolID = @id",
                     map: (knownIpPool, ipRange) =>
                     {
                         knownIpPool.IpRange = ipRange;
                         return knownIpPool;
                     },
                    param: new { id },
                    splitOn: "IPRangeStart").FirstOrDefault();
        }

        public KnownIpPool? GetByIpAddress(string ip)
        {
            return connection.Query<KnownIpPool, IpRange, KnownIpPool>(@$"SELECT {knownIpPoolFields}
                    FROM KnownIPPool where @ip >= IPRangeStartHex and @ip <= IPRangeEndHex ORDER BY DateAdded DESC",
                    map: (knownIpPool, ipRange) =>
                    {
                        knownIpPool.IpRange = ipRange; 
                        return knownIpPool;
                    },
                    param: new { ip = GeoIpUtils.IpToHex(ip) },
                    splitOn: "IPRangeStart").FirstOrDefault();
        }


        public void Add(KnownIpPool knownIpPool)
        {
            connection.Execute(@"INSERT INTO KnownIPPool ( IsGood, IsBlacklisted, Comments, IPRangeStart, IPRangeEnd, IPRangeStartHex, IPRangeEndHex, ApplicationID) 
                VALUES (@isGood, @isBlacklisted, @comments, @ipRangeStart, @ipRangeEnd, @startIpHex, @endIpHex, @appId)",
                new
                {
                    isGood = knownIpPool.IsGood,
                    isBlacklisted = knownIpPool.IsBlacklisted,
                    comments = knownIpPool.Comments,
                    ipRangeStart = knownIpPool.IpRange.IpRangeStart,
                    ipRangeEnd = knownIpPool.IpRange.IpRangeEnd,
                    startIpHex = GeoIpUtils.IpToHex(knownIpPool.IpRange.IpRangeStart),
                    endIpHex = GeoIpUtils.IpToHex(knownIpPool.IpRange.IpRangeEnd),
                    appId = knownIpPool.ApplicationId
                });

            knownIpPool.KnownIpPoolId = Convert.ToInt32(connection.ExecuteScalar("SELECT last_insert_rowid()"));

        }
    }
}
