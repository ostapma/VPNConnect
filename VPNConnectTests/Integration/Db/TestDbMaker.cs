using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnConnectTests.Integration.Db
{
    internal class TestDbMaker
    {
        private readonly string dbToCopyPath;
        private readonly string testDbPath;

        public TestDbMaker(string dbToCopyPath, string testDbPath)
        {
            this.dbToCopyPath = dbToCopyPath;
            this.testDbPath = testDbPath;
        }

        public void Make()
        {
            if (!File.Exists(testDbPath)) 
                File.Copy(dbToCopyPath, testDbPath);    
        }

        public void Cleanup()
        {
            if (File.Exists(testDbPath))
                File.Delete(testDbPath);
        }
    }
}
