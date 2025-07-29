using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Core.Entities.BibleVersionDetails;
using BibleStudyTool.Core.Globals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferences
{
    public static class BibleVerseHelper
    {
        private static readonly string[] OneChapterBooks = { "OBAD", "PHILM", "2JOHN", "3JOHN", "JUDE" };

        private static readonly Dictionary<string, string> BibleBookNameKeyBank = new Dictionary<string, string>()
        {
            // Old Testament

            {"genesis", "GEN" }, {"gen", "GEN" }, {"ge", "GEN" }, {"gn", "GEN" },
            {"exodus", "EXOD" }, {"exod", "EXOD" }, {"exo", "EXOD" }, {"ex", "EXOD" },
            {"leviticus", "LEV"}, {"lev", "LEV"}, {"le", "LEV"}, {"lv", "LEV"},
            {"numbers", "NUM"}, {"num", "NUM"}, {"nu", "NUM"}, {"nm", "NUM"}, {"nb", "NUM"},
            {"deuteronomy", "DEUT"}, {"deut", "DEUT"}, {"de", "DEUT"}, {"dt", "DEUT"},
            {"joshua", "JOSH"}, {"josh", "JOSH"}, {"jos", "JOSH"}, {"jsh", "JOSH"},
            {"judges", "JUDG"}, {"jdgs", "JUDG"}, {"judg", "JUDG"}, {"jdg", "JUDG"}, {"jg", "JUDG"},
            {"ruth", "RUTH"}, {"rth", "RUTH"}, {"ru", "RUTH"},
            {"1samuel", "1SAM"}, {"1sam", "1SAM"}, {"1sm", "1SAM"}, {"1sa", "1SAM"}, {"1s", "1SAM"},
            {"2samuel", "2SAM"}, {"2sam", "2SAM"}, {"2sm", "2SAM"}, {"2sa", "2SAM"}, {"2s", "2SAM"},
            {"1kings", "1KGS"}, {"1kgs", "1KGS"}, {"1ki", "1KGS"},
            {"2kings", "2KGS"}, {"2kgs", "2KGS"}, {"2ki", "2KGS"},
            {"1chronicles", "1CHR"}, {"1chron", "1CHR"}, {"1chr", "1CHR"}, {"1ch", "1CHR"},
            {"2chronicles", "2CHR"}, {"2chron", "2CHR"}, {"2chr", "2CHR"}, {"2ch", "2CHR"},
            {"ezra", "EZRA"}, {"ezr", "EZRA"}, {"ez", "EZRA"},
            {"nehemiah", "NEH"}, {"neh", "NEH"}, {"ne", "NEH"},
            {"esther", "ESTH"}, {"esth", "ESTH"}, {"es", "ESTH"},
            {"job", "JOB"}, {"jb", "JOB"},
            {"psalms", "PS"}, {"ps", "PS"}, {"psalm", "PS"}, {"pslm", "PS"}, {"psa", "PS"}, {"pss", "PS"},
            {"proverbs", "PROV"}, {"prov", "PROV"}, {"pro", "PROV"}, {"prv", "PROV"}, {"pr", "PROV"},
            {"ecclesiastes", "ECCL"}, {"eccles", "ECCL"}, {"eccle", "ECCL"}, {"eccl", "ECCL"}, {"ecc", "ECCL"}, {"ec", "ECCL"}, {"Qoh", "ECCL"},
            {"songofsongs", "SONG"}, {"songofsol", "SONG"}, {"songsol", "SONG"}, {"song", "SONG"}, {"sos", "SONG"}, {"so", "SONG"}, {"ss", "SONG"}, {"canticlesofcanticles", "SONG"}, {"canticles", "SONG"}, {"cant", "SONG"},
            {"isaiah", "ISA"}, {"isa", "ISA"}, {"is", "ISA"},
            {"jeremiah", "JER"}, {"jer", "JER"}, {"je", "JER"}, {"jr", "JER"},
            {"lamentations", "LAM"}, {"lam", "LAM"}, {"la", "LAM"},
            {"ezekiel", "EZEK"}, {"ezek", "EZEK"}, {"eze", "EZEK"}, {"ezk", "EZEK"},
            {"daniel", "DAN"}, {"dan", "DAN"}, {"da", "DAN"}, {"dn", "DAN"},
            {"hosea", "HOS"}, {"hos", "HOS"}, {"ho", "HOS"},
            {"joel", "JOEL"}, {"jl", "JOEL"},
            {"amos", "AMOS"}, {"am", "AMOS"},
            {"obadiah", "OBAD"}, {"obad", "OBAD"}, {"oba", "OBAD"}, {"ob", "OBAD"},
            {"jonah", "JONAH"}, {"jnh", "JONAH"}, {"jon", "JONAH"},
            {"micah", "MIC"}, {"mic", "MIC"}, {"mc", "MIC"},
            {"nahum", "NAH"}, {"nah", "NAH"}, {"na", "NAH"},
            {"habakkuk", "HAB"}, {"hab", "HAB"}, {"hb", "HAB"},
            {"zephaniah", "ZEPH"}, {"zeph", "ZEPH"}, {"zep", "ZEPH"}, {"zp", "ZEPH"},
            {"haggai", "HAG"}, {"hag", "HAG"}, {"hg", "HAG"},
            {"zechariah", "ZECH"}, {"zech", "ZECH"}, {"zec", "ZECH"}, {"zc", "ZECH"},
            {"malachi", "MAL"}, {"mal", "MAL"}, {"ml", "MAL"},

            // New Testament

            {"matthew", "MATT"}, {"matt", "MATT"}, {"mt", "MATT"},
            {"mark", "MARK"}, {"mrk", "MARK"}, {"mar", "MARK"}, {"mk", "MARK"}, {"mr", "MARK"},
            {"luke", "LUKE"}, {"luk", "LUKE"}, {"lk", "LUKE"},
            {"john", "JOHN"}, {"joh", "JOHN"}, {"jhn", "JOHN"}, {"jn", "JOHN"},
            {"acts", "ACTS"}, {"act", "ACTS"}, {"ac", "ACTS"},
            {"romans", "ROM"}, {"rom", "ROM"}, {"ro", "ROM"}, {"rm", "ROM"},
            {"1corinthians", "1COR"}, {"1cor", "1COR"}, {"1co", "1COR"},
            {"2corinthians", "2COR"}, {"2cor", "2COR"}, {"2co", "2COR"},
            {"galatians", "GAL"}, {"gal", "GAL"}, {"ga", "GAL"},
            {"ephesians", "EPH"}, {"eph", "EPH"}, {"ephes", "EPH"},
            {"philippians", "PHIL"}, {"phil", "PHIL"}, {"php", "PHIL"}, {"pp", "PHIL"},
            {"colossians", "COL"}, {"col", "COL"}, {"co", "COL"},
            {"1thessalonians", "1THESS"}, {"1thess", "1THESS"}, {"1thes", "1THESS"}, {"1th", "1THESS"},
            {"2thessalonians", "2THESS"}, {"2thess", "2THESS"}, {"2thes", "2THESS"}, {"2th", "2THESS"},
            {"1timothy", "1TIM"}, {"1tim", "1TIM"}, {"1ti", "1TIM"},
            {"2timothy", "2TIM"}, {"2tim", "2TIM"}, {"2ti", "2TIM"},
            {"titus", "TITUS"}, {"tit", "TITUS"}, {"ti", "TITUS"},
            {"philemon", "PHLM"}, {"philem", "PHLM"}, {"phlm", "PHLM"}, {"phm", "PHLM"}, {"pm", "PHLM"},
            {"hebrews", "HEB"}, {"heb", "HEB"},
            {"james", "JAS"}, {"jas", "JAS"}, {"jm", "JAS"},
            {"1peter", "1PET"}, {"1pet", "1PET"}, {"1pt", "1PET"}, {"1p", "1PET"},
            {"2peter", "2PET"}, {"2pet", "2PET"}, {"2pt", "2PET"}, {"2p", "2PET"},
            {"1john", "1JOHN"}, {"1jhn", "1JOHN"}, {"1jn", "1JOHN"}, {"1j", "1JOHN"},
            {"2john", "2JOHN"}, {"2jhn", "2JOHN"}, {"2jn", "2JOHN"}, {"2j", "2JOHN"},
            {"3john", "3JOHN"}, {"3jhn", "3JOHN"}, {"3jn", "3JOHN"}, {"3j", "3JOHN"},
            {"jude", "JUDE"}, {"jud", "JUDE"}, {"jd", "JUDE"},
            {"revelation", "REV"}, {"rev", "REV"}, {"re", "REV"}
        };

        public enum ParseMatchGroupPosition
        {
            WholeMatch,
            Book,
            Chapter,
            Verses
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Separates semicolon-separated verse references into verse reference units.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     A MatchCollection of string verse references.
        /// </returns>
        private static MatchCollection TokenizeVerseReferences(string rawVerseReferences)
        {
            string pattern = @"((?:[123]\s)?[a-zA-Z]+\.?)?(\s\d+-?\d*:?(?!\s\w*))(\d+[a-z]?-?\d*(?:,)?\s?)*";
            RegexOptions regexOptions = RegexOptions.IgnoreCase;

            Regex regex =
                new(pattern, regexOptions, TimeSpan.FromSeconds(1));

            return regex.Matches(rawVerseReferences);
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Converts the Bible book name into its associated Bible book key.
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="testBookName"></param>
        /// <returns>
        ///     Bible book key.
        /// </returns>
        /// <exception cref="Exception"></exception>
        internal static string NormalizeBookName(string bookName, string testBookName)
        {
            bookName =
                Regex.Replace(
                    (string.IsNullOrEmpty(testBookName)
                        ? bookName
                        : testBookName)
                    .ToLower(),
                    @"\.|\s",
                    ""
                );

            string result = string.Empty;

            if (string.IsNullOrEmpty(bookName) || !BibleBookNameKeyBank.TryGetValue(bookName, out result!))
                throw new Exception
                    ($"Parser encountered a verse reference with an unknown Bible book name: {bookName}.");

            return result;
        }

        public static bool IsValidChapter(string bookKey, int chapterNumber)
        {
            // check if chapter is in the book
            // first check if the book is a one-chapter book
            if (HasOneChapter(bookKey) && chapterNumber > 1) return false;

            //BibleVersionsDetailsStore.GetBibleVersionInformation

            return true;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="bookKey"></param>
        /// <param name="chapterNumber"></param>
        /// <returns></returns>
        public static bool IsChapterInBook(string bookKey, int chapterNumber)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bibleBookKey"></param>
        /// <returns></returns>
        internal static bool HasOneChapter(string bibleBookKey)
            => OneChapterBooks.Contains(bibleBookKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verseNumber"></param>
        /// <returns></returns>
        public static int NumerizeNumericComponent(string verseNumber)
        {
            if (string.IsNullOrEmpty(verseNumber)) return 0;

            string cleanedVerse = Regex.Replace(verseNumber.Trim(), @"\D*", "");

            if (int.TryParse(cleanedVerse, out int result)) return result;

            throw new Exception
                ($"Could not numerize numeric component: {verseNumber}");
        }
    }
}
