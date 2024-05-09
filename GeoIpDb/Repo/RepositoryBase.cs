using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Repo
{
    public abstract class RepositoryBase:IDisposable
    {
        protected SqliteConnection connection;

        public void SetSynchronousModeOff()
        {
            connection.Execute("PRAGMA synchronous=OFF");
        }
        public RepositoryBase(string connectionString)
        {

            connection = new SqliteConnection(connectionString);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
