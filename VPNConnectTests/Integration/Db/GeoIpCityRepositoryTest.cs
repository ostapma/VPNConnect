using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPNConnect.Net;
using GeoIpDb.Repo;
using GeoIpDb.Entities;

namespace VpnConnectTests.Integration.Db
{
    public class GeoIpCityRepositoryTest: GeoIpRepositoryTestBase<GeoIpCityRepository>
    {
        

        [Test]
        public void GetCountTest()
        {
            var actual = repo.GetCount();
            Assert.AreNotEqual(0, actual);
        }

        [Test]
        public void GetByIpAddressTest()
        {
            var actual = repo.GetByIpAddress(TestSettings.GoogleDnsIp);
            CheckAllFieldsFilled(actual);
            Assert.AreEqual("US", actual.CountryID);
        }

        [Test]
        public void GetByIdTest()
        {
            var testData = repo.GetByIpAddress(TestSettings.GoogleDnsIp);
            var actual = repo.GetById(testData.CityIpId);
            CheckAllFieldsFilled(actual);
            Assert.AreEqual("US", actual.CountryID);
        }

        [Test]
        public void GetPageTest()
        {
            var testData = repo.GetByIpAddress(TestSettings.GoogleDnsIp);
            var actual = repo.GetPage(testData.CityIpId, TestSettings.TestPageSize);
            Assert.AreEqual(actual.Count(), TestSettings.TestPageSize);
            foreach (var a in actual)   CheckAllFieldsFilled(a);
        }

        [Test]
        public void UpdateHexIpTest()
        {
            var testData = repo.GetByIpAddress(TestSettings.GoogleDnsIp);
            var testIpRange = new IpRange { IpRangeStart = "255.255.255.0", IpRangeEnd = "255.255.255.255" };
            repo.UpdateHexIp(testData.CityIpId, testIpRange);
            var updatedData = repo.GetByIpAddress("255.255.255.1");
            CheckAllFieldsFilled(updatedData);
            repo.UpdateHexIp(testData.CityIpId, testData.IpRange);
        }

        private void CheckAllFieldsFilled(GeoIpCity geoIpCity)
        {
            Assert.IsNotNull(geoIpCity);
            Assert.IsNotNull(geoIpCity.CityName);
            Assert.IsNotNull(geoIpCity.ContinentID);
            Assert.IsNotNull(geoIpCity.CityIpId);
            Assert.IsNotNull(geoIpCity.CountryID);
            Assert.IsNotNull(geoIpCity.IpRange);
            Assert.IsNotNull(geoIpCity.IpRange.IpRangeStart);
            Assert.IsNotNull(geoIpCity.IpRange.IpRangeEnd);
        }

    }
}
