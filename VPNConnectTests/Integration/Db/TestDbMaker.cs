using GeoIpDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    public class TestDbMaker
    {
        private readonly string dbToCopyPath;
        private readonly string testDbPath;

        const string dbFileName = "geoip.db";
        const string testDbFileName = "geoip.test.db";
        public TestDbMaker()
        {
            var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dbToCopyPath = Path.GetFullPath(Path.Combine(currentPath, "..", "..", "..", "..", dbFileName));
            testDbPath = Path.Combine(currentPath, testDbFileName);
        }

        public string Make()
        {
            if (!File.Exists(testDbPath)) 
                File.Copy(dbToCopyPath, testDbPath);    
            return testDbPath;
        }

        public void Cleanup()
        {
            SqliteDbManager.ReleaseAllDbFiles();
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}
