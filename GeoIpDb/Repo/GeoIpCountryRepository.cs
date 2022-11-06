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

        public IEnumerable<GeoIpCountry> GetList()
        {
            return connection.Query<GeoIpCountry>(@"SELECT CountryID, Name, IsBlacklisted FROM Country");
        }
    }
}
