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
                ApplicationId = 1
            };

            repo.Add(testIpPool);

            var actual = repo.GetByIpAddress("255.255.255.1");

            Assert.NotNull(actual);
            Assert.AreEqual(testIpPool.IpRange, actual.IpRange);
            Assert.AreEqual(testIpPool.IsGood, actual.IsGood);
            Assert.AreEqual(testIpPool.Comments, actual.Comments);
            Assert.AreEqual(testIpPool.IsBlacklisted, actual.IsBlacklisted);
            Assert.AreEqual(testIpPool.ApplicationId, actual.ApplicationId );
            Assert.IsNotNull(actual.DateAdded);

        }
    }
}
