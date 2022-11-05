using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb
{
    public class SqliteDbManager
    {
        //Fix for: SQLite keeps the database locked even after the connection is closed
        //https://stackoverflow.com/questions/12532729/sqlite-keeps-the-database-locked-even-after-the-connection-is-closed
        public static void ReleaseAllDbFiles()
        {
            SqliteConnection.ClearAllPools();
        }
    }
}
