using BibleStudyTool.Core.Entities.BibleVerse;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Infrastructure.DAL.Npgsql
{
    public class BibleVerseQueries :  EntityQueries
    {
        public BibleVerseQueries(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<BibleVerse>> SearchVerseReferencesQueryAsync(IEnumerable<(string, int, int)> verseReferenceUnits)
        {

            throw new NotImplementedException("This method is not implemented yet.");
            //using (NpgsqlConnection sqlCnx = GetConnection())
            //using (NpgsqlCommand sqlCmd = new NpgsqlCommand(string.Empty, sqlCnx))
            //{
            //    //sqlCmd.CommandText 
            //}
        }
    }
}
