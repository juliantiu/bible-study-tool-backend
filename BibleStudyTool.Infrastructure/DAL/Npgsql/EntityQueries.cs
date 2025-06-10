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
        protected readonly string _connectionString;

        public EntityQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected NpgsqlConnection GetConnection()
        {
            NpgsqlConnection sqlCnx = new(_connectionString);
            sqlCnx.Open();
            return sqlCnx;
        }
    }
}
