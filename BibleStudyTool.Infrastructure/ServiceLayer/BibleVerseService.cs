using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Core.Entities.BibleVersionInformation;
using BibleStudyTool.Core.Globals;
using BibleStudyTool.Core.Utilities;
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
        public Task<IEnumerable<BibleVerse>> GetChapterVerses(string language, string version, string bookKey, int chapter)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BibleVerse>> SearchVerseReferences(string language, string versionAbbreviation, string rawVerseReferences)
        {
            try
            {
                IEnumerable<(string, string, string)> parsedVerses = BibleVerseHelper.ParseSSVR(rawVerseReferences);

                // BibleVersionInformation? bibleVersion = BibleVersionLookupTable.BibleVersionInformationRequest(language, versionAbbreviation);

                Console.WriteLine("hello world");

            }
            catch (ArgumentNullException)
            {
                throw new Exception("Bible version does not exist.");
            }

            throw new NotImplementedException();
        }

        public Task<BibleVerse> GetVerse(string verseReferenceKey, string language, string version)
        {
            throw new NotImplementedException();
        }
    }
}
