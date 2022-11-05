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
        string conn = "Data Source=C:\\Work\\Winsconnect\\VpnConnect\\geoip.db";


        [Test]
        public void GetCountTest()
        {
            GeoIpRepository repo = new GeoIpRepository(conn);
            var actual = repo.GetCount();
            Assert.AreNotEqual(0, actual);
        }

        [Test]
        public void GetByIpAddressTest()
        {
            GeoIpRepository repo = new GeoIpRepository(conn);
            var actual = repo.GetByIpAddress("8.8.8.8");
            Assert.IsNotNull(actual);
            Assert.AreEqual("US", actual.CountryID);
        }

    }
}
