using GeoIpDb.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    public abstract class GeoIpRepositoryTestBase<T> where T : RepositoryBase
    {
        protected T repo;

        [SetUp]
        public void Init()
        {
            repo = (T)Activator.CreateInstance(typeof(T), SqliteTestDbScope.GetTestConnectionString());
        }

        [TearDown]
        public void TearDown()
        {
            repo.Dispose();
        }

    }
}
