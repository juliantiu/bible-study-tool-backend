using BibleStudyTool.Core.Entities.BibleVerse;
using Npgsql;
using NpgsqlTypes;
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

        public async Task<IEnumerable<BibleVerse>>
            SearchVerseReferencesQueryAsync
                (IEnumerable<BibleVerse> verses)
        {
            List<BibleVerse> bibleVerses = new List<BibleVerse>();

            using (NpgsqlConnection sqlCnx = GetConnection())
            using (NpgsqlCommand sqlCmd = new NpgsqlCommand(string.Empty, sqlCnx))
            {               
                // Add parameters for each verse reference
                foreach (var verse in verses)
                {

                    sqlCmd.CommandText =
@"SELECT verse_text
FROM bible_verses
WHERE
    language = @language
    AND version_abbreviation = @versionAbbreviation
    AND book_key = @bookKey
    AND chapter_number = @chapterNumber
    AND verse_number = @verseNumber;";

                    sqlCmd.Parameters.Clear();

                    sqlCmd.Parameters.AddWithValue("@language", verse.Language);
                    sqlCmd.Parameters
                        .AddWithValue
                            ("@versionAbbreviation", verse.VersionAbbreviation);
                    sqlCmd.Parameters.AddWithValue("@bookKey", verse.BookKey);
                    sqlCmd.Parameters
                        .AddWithValue("@chapterNumber", NpgsqlDbType.Integer, verse.ChapterNumber);
                    sqlCmd.Parameters
                        .AddWithValue("@verseNumber", NpgsqlDbType.Integer, verse.VerseNumber);

                    using (NpgsqlDataReader reader = await sqlCmd.ExecuteReaderAsync())
                    { 
                        while (await reader.ReadAsync())
                        {
                            verse
                                .OverrideVerseText
                                    (reader.GetString
                                        (reader.GetOrdinal("verse_text"))
                                        ?? "");

                            bibleVerses.Add(verse);
                        }
                    }
                }
            
                return bibleVerses;
            }
        }
    }
}
