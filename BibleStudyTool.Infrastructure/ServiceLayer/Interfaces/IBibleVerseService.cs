using BibleStudyTool.Core.Entities.BibleVerse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Infrastructure.ServiceLayer.Interfaces
{
    public interface IBibleVerseService
    {
        /// <summary>
        ///     Retrieves one specific Bible verse.
        /// </summary>
        /// <param name="verseReferenceKey"></param>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<BibleVerse> GetVerse
            (string verseReferenceKey, string language, string version);

        /// <summary>
        ///     Gets all the verses for a Bible book chapter.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chapter"></param>
        /// <returns></returns>
        Task<IEnumerable<BibleVerse>> GetChapterVerses
            (string language, string version, string bookKey, int chapter);

        /// <summary>
        ///     Gets all the verses specified by a string of
        ///     semi-colon separated verse references.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="versionAbbreviation"></param>
        /// <param name="rawVerseReferences"></param>
        /// <returns></returns>
        Task<IEnumerable<BibleVerse>> SearchVerseReferences
            (string language, string versionAbbreviation, string rawVerseReferences);
    }
}
