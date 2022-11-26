using GeoIpDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    internal class TargetApplicationRepositoryTest: GeoIpRepositoryTestBase<GeoIpDb.Repo.TargetApplicationRepository>
    {
        [Test]
        public void AddTest()
        {
            var testApplication = AddTestApplication();

            var actual = repo.GetById(testApplication.ApplicationId);

            Assert.NotNull(actual);
            Assert.AreNotEqual(0, actual.ApplicationId);
            Assert.AreEqual(testApplication.Name, actual.Name);
            Assert.AreEqual(testApplication.ApplicationId, actual.ApplicationId);
        }

        [Test]
        public void RemoveTest()
        {
            var testApplication = AddTestApplication();

            repo.Remove(testApplication.ApplicationId);

            var actual = repo.GetById(testApplication.ApplicationId);

            Assert.IsNull(actual);
        }

        private TargetApplication AddTestApplication()
        {
            var testApplication = new TargetApplication()
            {
                Name = Guid.NewGuid().ToString(),
            };

            repo.Add(testApplication);
            return testApplication;
        }
    }
}
