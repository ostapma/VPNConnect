using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.GeoIp;
using VPNConnect.Net;

namespace VPNConnectTests.Integration
{
    [TestClass]
    public class GeoIpRepositoryTest
    {
        string conn = "Data Source=C:\\Work\\Winsconnect\\VpnConnect\\geoip.db";


        [TestMethod]
        public void GetCountTest()
        {
            GeoIpRepository repo = new GeoIpRepository(conn);
            var actual = repo.GetCount();
            Assert.AreNotEqual(0, actual);
        }

        [TestMethod]
        public void GetByIpAddressTest()
        {
            GeoIpRepository repo = new GeoIpRepository(conn);
            var actual = repo.GetByIpAddress(NetUtils.IpToInt("8.8.8.8"));
            Assert.IsNotNull(actual);
            Assert.AreEqual("US", actual.CountryID);
        }

    }
}
