using Dapper;
using GeoIpDb.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Repo
{
    public class GeoIpCountryRepository : RepositoryBase
    {
        public GeoIpCountryRepository(string connectionString) : base(connectionString)
        {
        }

        public List<GeoIpCountry> GetList()
        {
            return connection.Query<GeoIpCountry>(@"SELECT CountryID, Name  FROM Country").ToList();
        }

        public List<GeoIpCountry> GetBlacklistedList(int applicationId)
        {
            return connection.Query<GeoIpCountry>(@"SELECT c.CountryID, c.Name  FROM Country c join CountryBlacklist l on 
                c.CountryID = l.CountryID where l.ApplicationID = applicationID",
                param: new { applicationId }).ToList();
        }
    }
}
