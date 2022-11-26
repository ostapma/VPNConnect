using Dapper;
using GeoIp;
using GeoIpDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIpDb.Repo
{
    public class TargetApplicationRepository : RepositoryBase
    {
        string fields = "ApplicationID, Name";

        public TargetApplicationRepository(string connectionString) : base(connectionString)
        {
        }

        public TargetApplication? GetById(int id)
        {
            return connection.Query<TargetApplication>(@$"SELECT {fields}
                    FROM Application where ApplicationID = @id and IsDeleted = 0",
                    param: new { id }).FirstOrDefault();
        }


        public void Add(TargetApplication application)
        {
            connection.Execute(@"INSERT INTO Application ( Name) 
                VALUES (@name)",
               new
               {
                   name = application.Name                
               });
            application.ApplicationId =Convert.ToInt32( connection.ExecuteScalar("SELECT last_insert_rowid()"));
        }

        public void Remove(int applicationId)
        {
            connection.Execute(@"UPDATE Application set IsDeleted = 1 where ApplicationID = @id",
               new
               {
                   id = applicationId
               });
        }
    }
}
