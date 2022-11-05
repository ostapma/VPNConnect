using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;
using GeoIp.Repo;

namespace VpnConnectTests.Integration.Db
{
    public class GeoIpRepositoryTest
    {
        private GeoIpRepository repo;

        [SetUp]
        public void Init()
        {
            repo = new GeoIpRepository(SqliteTestDbScope.GetTestConnectionString());
        }

        [TearDown]
        public void TearDown()
        {
            repo.Dispose();
        }


        [Test]
        public void GetCountTest()
        {
            var actual = repo.GetCount();
            Assert.AreNotEqual(0, actual);
        }

        [Test]
        public void GetByIpAddressTest()
        {
            var actual = repo.GetByIpAddress("8.8.8.8");
            Assert.IsNotNull(actual);
            Assert.AreEqual("US", actual.CountryID);
        }

    }
}
