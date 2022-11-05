using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    [SetUpFixture]
    public class Initializer
    {
        const string dbFileName = "geoip.db";
        const string testDbFileName = "geoip.test.db";

        TestDbMaker testDbMaker;

        public Initializer()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dbPath = Path.GetFullPath(Path.Combine(currentPath, "..", "..", "..", "..", dbFileName));
            var testDbPath = Path.Combine(currentPath, testDbFileName);
            testDbMaker = new TestDbMaker(dbPath, testDbPath);
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            testDbMaker.Make();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            testDbMaker.Cleanup();
        }
    }
}
