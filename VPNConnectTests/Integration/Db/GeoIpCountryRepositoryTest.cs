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
            Assert.IsNotNull(actual);
            Assert.AreNotEqual(actual.Count(),0);
            var firstItem = actual.First();
            Assert.IsNotNull(firstItem.Name);
            Assert.IsNotNull(firstItem.CountryId);
            Assert.IsNotEmpty(firstItem.Name);
            Assert.IsNotEmpty(firstItem.CountryId);
        }
    }
}
