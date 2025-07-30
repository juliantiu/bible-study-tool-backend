using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Infrastructure.DAL.Npgsql
{
    public abstract class EntityQueries
    {
        protected readonly string ConnectionString;

        public EntityQueries(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected NpgsqlConnection GetConnection()
        {
            NpgsqlConnection sqlCnx = new(ConnectionString);
            sqlCnx.Open();
            return sqlCnx;
        }
    }
}
