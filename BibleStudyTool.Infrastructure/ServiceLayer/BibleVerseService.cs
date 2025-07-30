using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Core.Entities.BibleVersionDetails;
using BibleStudyTool.Core.Globals;
using BibleStudyTool.Core.Utilities;
using BibleStudyTool.Core.Utilities.BibleVerseReferences;
using BibleStudyTool.Infrastructure.DAL.Npgsql;
using BibleStudyTool.Infrastructure.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Infrastructure.ServiceLayer
{
    public class BibleVerseService : IBibleVerseService
    {

        private readonly BibleVerseQueries BibleVerseQueries;

        public BibleVerseService(BibleVerseQueries bibleVerseQueries)
        {
            BibleVerseQueries = bibleVerseQueries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chapter"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<BibleVerse>>
            GetChapterVerses
                (string language, string version, string bookKey, int chapter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="versionAbbreviation"></param>
        /// <param name="rawVerseReferences"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<BibleVerse>>
            SearchVerseReferences
                (string language,
                string versionAbbreviation,
                string rawVerseReferences)
        {
            try
            {
                
                BibleVerseReferencesProcessor verseReferencesProcessor
                    = new(language, versionAbbreviation, rawVerseReferences);

                List<BibleVerse> bibleVerses =
                    verseReferencesProcessor
                        .ParseBibleVerseReferencesInput(rawVerseReferences);

                return await BibleVerseQueries
                            .SearchVerseReferencesQueryAsync(bibleVerses);

            }
            catch (ArgumentNullException)
            {
                throw new Exception("Bible version does not exist.");
            }

            throw new NotImplementedException();
        }

        public Task<BibleVerse> GetVerse
            (string verseReferenceKey, string language, string version)
        {
            throw new NotImplementedException();
        }
    }
}
