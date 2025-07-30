using BibleStudyTool.Core.Entities.BibleVersionDetails;

namespace BibleStudyTool.Core.Globals
{
    public static class BibleVersionsDetailsStore
    {
        private static readonly Dictionary
            <string, Dictionary<string, BibleVersionDetails>>
                _bibleVersionsInformationStore = new();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bibleVersionInformation"></param>
        public static void
            AddBibleVersionInformation
                (BibleVersionDetails bibleVersionInformation)
        {
            string language = bibleVersionInformation.Language;
           
            string versionAbbreviation =
                bibleVersionInformation.VersionAbbreviation;

            if (!_bibleVersionsInformationStore.ContainsKey(language))
            {
                _bibleVersionsInformationStore[language] =
                    new Dictionary<string, BibleVersionDetails>();
            }
            _bibleVersionsInformationStore[language][versionAbbreviation] =
                bibleVersionInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="versionAbbreviation"></param>
        /// <returns></returns>
        public static BibleVersionDetails?
            GetBibleVersionInformation
                (string language, string versionAbbreviation)
        {
            if (_bibleVersionsInformationStore
                    .TryGetValue(language, out var versions)
                && versions
                    .TryGetValue(versionAbbreviation, out var bibleVersionInfo))
            {
                return bibleVersionInfo;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chapter"></param>
        /// <param name="totalChapters"></param>
        /// <returns></returns>
        public static bool IsChapterInBook
            (string language,
            string version,
            string bookKey,
            string chapter)
        {
            if (_bibleVersionsInformationStore
                .TryGetValue(language, out var versions))
            {
                if (versions.TryGetValue(version, out var bibleVersionInfo))
                {
                    if (bibleVersionInfo
                        .BibleBookDefinitions
                            .TryGetValue(bookKey, out var bookDefinition))
                    {
                        int chapterNumbers = bookDefinition.ChapterCount;

                        return Int32.TryParse(chapter, out int chapterNumber)
                            && chapterNumber > 0
                            && chapterNumber <= chapterNumbers;
                    }

                    // TODO: Log this error
                    // (@$"{bookKey} does not exist in version {version}" +
                    // $"of language {language}.");
                }

                // TODO: Log this error
                // ($"Version {version} does not exist for language" +
                // $"{language}.");
            }

            // TODO: Log this error
            // ($"Language {language} does not yet exist in the Bible" +
            // $"repository.");

            return false;
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
        public static int GetTotalVersesInChapter
            (string language,
            string version,
            string bookKey,
            string chapter)
        {
            if (_bibleVersionsInformationStore
                    .TryGetValue(language, out var versions))
            {
                if (versions.TryGetValue(version, out var bibleVersionInfo))
                {
                    if (bibleVersionInfo
                        .BibleBookDefinitions
                            .TryGetValue(bookKey, out var bookDefinition))
                    {
                        if(bookDefinition
                            .VerseCountPerChapter
                                .TryGetValue
                                    (Int32.Parse(chapter), out int totalVerses))
                        {
                             return totalVerses;
                        }

                        // TODO: Log this error
                        // (@$"{chapter} does not exist in {bookKey} +
                        // $"of version {version}" +
                        // $"of language {language}.");

                    }

                    // TODO: Log this error
                    // (@$"{bookKey} does not exist in version {version}" +
                    // $"of language {language}.");
                }

                // TODO: Log this error
                // ($"Version {version} does not exist for language" +
                // $"{language}.");
            }

            // TODO: Log this error
            // ($"Language {language} does not yet exist in the Bible" +
            // $"repository.");

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chapter"></param>
        /// <param name="verse"></param>
        /// <returns></returns>
        public static bool IsVerseInChapter
            (string language,
            string version,
            string bookKey,
            string chapter,
            string verse)
        {

            int numVersesInChapter =
                GetTotalVersesInChapter
                    (language,
                    version,
                    bookKey,
                    chapter);

            return Int32.TryParse(verse, out int verseNumber)
                && verseNumber > 0
                && verseNumber <= numVersesInChapter;
        }
    }
}
