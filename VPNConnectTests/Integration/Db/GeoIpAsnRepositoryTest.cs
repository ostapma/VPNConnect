using GeoIp.Repo;
using GeoIpDb.Entities;
using GeoIpDb.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    public class GeoIpAsnRepositoryTest:GeoIpRepositoryTestBase
    {
        private GeoIpAsnRepository repo;

        [SetUp]
        public void Init()
        {
            repo = new GeoIpAsnRepository(SqliteTestDbScope.GetTestConnectionString());
        }

        [TearDown]
        public void TearDown()
        {
            repo.Dispose();
        }

        [Test]
        public void GetByIpAddressTest()
        {
            var actual = repo.GetByIpAddress(googleDnsIp);
            CheckAllFieldsFilled(actual);
        }

        [Test]
        public void GetPageTest()
        {
            var testData = repo.GetByIpAddress(googleDnsIp);
            var actual = repo.GetPage(testData.AsnIpId, 10);
            Assert.AreEqual(actual.Count(), 10);
            foreach (var a in actual) CheckAllFieldsFilled(a);
        }

        [Test]
        public void GetCountTest()
        {
            var actual = repo.GetCount();
            Assert.AreNotEqual(0, actual);
        }

        [Test]
        public void UpdateHexIpTest()
        {
            var testData = repo.GetByIpAddress(googleDnsIp);
            var testIpRange = new IpRange { IpRangeStart = "255.255.255.0", IpRangeEnd = "255.255.255.255" };
            repo.UpdateHexIp(testData.AsnIpId, testIpRange);
            var updatedData = repo.GetByIpAddress("255.255.255.1");
            CheckAllFieldsFilled(updatedData);
            repo.UpdateHexIp(testData.AsnIpId, testData.IpRange);
        }

        private void CheckAllFieldsFilled(GeoIpAsnIp geoIpAsnIp)
        {
            Assert.IsNotNull(geoIpAsnIp);
            Assert.IsNotNull(geoIpAsnIp.Asn);
            Assert.AreNotEqual(geoIpAsnIp.Asn.AsnId,0);
            Assert.IsNotNull(geoIpAsnIp.Asn.Name);
            Assert.IsNotNull(geoIpAsnIp.IpRange);
            Assert.IsNotNull(geoIpAsnIp.IpRange.IpRangeEnd);
            Assert.IsNotNull(geoIpAsnIp.IpRange.IpRangeStart);
        }

    }
}
