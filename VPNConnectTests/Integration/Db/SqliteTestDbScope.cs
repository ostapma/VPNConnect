using Autofac;
using GeoIpDb;
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
    public class SqliteTestDbScope
    {


        static string connString;

        TestDbMaker testDbMaker;

        public static string GetTestConnectionString()
        {
            return connString;
        }

        [OneTimeSetUp]
        public void Initialize()
        {

            testDbMaker = new TestDbMaker();
            testDbMaker.Cleanup();
            string testDbPath = testDbMaker.Make();
            connString = $"Data Source = {testDbPath}";
           
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            testDbMaker.Cleanup();
            connString = "";
        }
    }
}
