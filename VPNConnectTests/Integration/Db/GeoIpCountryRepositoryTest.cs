using GeoIpDb.Entities;
using GeoIpDb.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    public class GeoIpCountryRepositoryTest : GeoIpRepositoryTestBase<GeoIpCountryRepository>
    {
        [Test]
        public void GetListTest()
        {
            var actual = repo.GetList();
            BasicDataTest(actual);
        }

        [Test]
        public void GetBlacklistedTest()
        {
            int applicationId = 1;
            var actual = repo.GetBlacklistedList(applicationId);
            BasicDataTest(actual);

        }

        private void BasicDataTest(List<GeoIpCountry> countries)
        {
            Assert.IsNotNull(countries);
            Assert.AreNotEqual(countries.Count(), 0);
            var firstItem = countries.First();
            Assert.IsNotNull(firstItem.Name);
            Assert.IsNotNull(firstItem.CountryId);
            Assert.IsNotEmpty(firstItem.Name);
            Assert.IsNotEmpty(firstItem.CountryId);
        }
    }
}
