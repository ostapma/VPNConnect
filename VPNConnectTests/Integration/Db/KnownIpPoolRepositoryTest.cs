using GeoIpDb.Entities;
using GeoIpDb.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    internal class KnownIpPoolRepositoryTest : GeoIpRepositoryTestBase<KnownIpPoolRepository>
    {
        [Test]
        public void AddTest()
        {
            var testIpRange = new IpRange { IpRangeStart = "255.255.255.0", IpRangeEnd = "255.255.255.255" };

            var testIpPool = new KnownIpPool()
            {
                Comments = Guid.NewGuid().ToString(),
                IpRange = testIpRange,
                IsBlacklisted = true,
                IsGood = true,
            };

            repo.Add(testIpPool);

            var actual = repo.GetByIpAddress("255.255.255.1");

            Assert.NotNull(actual);
            Assert.AreEqual(actual.IpRange, testIpPool.IpRange);
            Assert.AreEqual(actual.IsGood, testIpPool.IsGood);
            Assert.AreEqual(actual.Comments, testIpPool.Comments);
            Assert.AreEqual(actual.IsBlacklisted, testIpPool.IsBlacklisted);
            Assert.IsNotNull(actual.DateAdded);

        }
    }
}
