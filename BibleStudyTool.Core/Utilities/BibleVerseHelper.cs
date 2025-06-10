using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Core.Entities.BibleVersionInformation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Utilities
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

        /// <summary>
        ///     Converts raw string of semicolon-separated verse references (SSVR) into list of verse references, which can be used to build a list of Bible verses.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     An enumerable set of tuples that describe Bible verse references.
        /// </returns>
        public static IEnumerable<(string, string, string)> ParseSSVR(string rawVerseReferences)
        {
            MatchCollection separatedVerses = SeparateSSVR(rawVerseReferences);
            return GenerateListOfVerseReferenceUnits(separatedVerses);
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Separates semicolon-separated verse references into verse reference units.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     A MatchCollection of string verse references.
        /// </returns>
        private static MatchCollection SeparateSSVR(string rawVerseReferences)
        {
            string pattern = @"\b([1-3]?\s?[A-Za-z]+\.?)?\s*(\d{1,3})\s*(?::)?\s*(\d{1,3}[a-z]?(?:[,|-]\s*\d{1,3}[a-z]?)*)?";
            RegexOptions regexOptions = RegexOptions.IgnoreCase;

            Regex regex =
                new(pattern, regexOptions, TimeSpan.FromSeconds(1));

            return regex.Matches(rawVerseReferences);
        }

        public enum ParseMatchGroupPosition
        {
            WholeMatch,
            Book,
            Chapter,
            Verses
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Organizes a set of raw string semicolon-separated verse references into an iterable set of tuples that describe Bible verse references.
        /// </summary>
        /// <param name="separatedVerses"></param>
        /// <returns>
        ///     An enumerable set of tuples that describe Bible verse references by (BookKey, ChapterNumber, VerseNumber).
        /// </returns>
        /// <exception cref="Exception"></exception>
        private static IEnumerable<(string, string, string)>
            GenerateListOfVerseReferenceUnits(MatchCollection separatedVerses)
        {
            List<(string, string, string)> parsedVerses = new();

            string bookName = String.Empty;

            foreach (Match separatedVerse in separatedVerses)
            {
                GroupCollection separatedVerseGroupings = separatedVerse.Groups;

                string matchedBookName =
                    separatedVerseGroupings[(int)ParseMatchGroupPosition.Book].Value;

                bookName = NormalizeBookName(bookName, matchedBookName);

                string chapter =
                        separatedVerseGroupings[(int)ParseMatchGroupPosition.Chapter].Value;

                string verses =
                    separatedVerseGroupings[(int)ParseMatchGroupPosition.Verses].Value;

                if (String.IsNullOrEmpty(verses) && OneChapterBooks.Contains(bookName))
                {
                    verses = chapter;
                    chapter = "1"; // Book only contains one chapter
                }

                SplitCommaSeparatedVerseNumbers(verses)
                    .Aggregate
                    (
                        parsedVerses,
                        (result, verse) =>
                        {
                            result.Add((bookName, chapter, verse));
                            return result;
                        }
                    );
            }

            return parsedVerses;
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Converts the Bible book name into its associated Bible book key.
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="matchedBookName"></param>
        /// <returns>
        ///     Bible book key.
        /// </returns>
        /// <exception cref="Exception"></exception>
        private static string NormalizeBookName(string bookName, string matchedBookName)
        {
            bookName =
                (String.IsNullOrEmpty(matchedBookName)
                    ? bookName
                    : matchedBookName)
                .ToLower()
                .Trim();

            string result = String.Empty;

            if (String.IsNullOrEmpty(bookName) || !BibleBookNameKeyBank.TryGetValue(bookName, out result!))
                throw new Exception
                    ($"Parser encountered a verse reference with an unknown Bible book name: {bookName}.");

            return result;
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Splits verse numbers in the raw string input of verse numbers that are separated by commas.
        /// </summary>
        /// <param name="verses"></param>
        /// <returns>
        ///     An enumerable set of verse numbers. 
        /// </returns>
        private static IEnumerable<string> SplitCommaSeparatedVerseNumbers(string verses)
        {
            return verses
                .Split(",")
                .Aggregate
                    (new List<string>(),
                    (listOfVerses, verse) =>
                    {
                        string[] splitByDash = verse.Split('-');

                        string firstValue = splitByDash[0];

                        listOfVerses.Add(firstValue.Replace(",", "").Trim());

                        if (splitByDash.Length <= 1) return listOfVerses;

                        string lastValue = splitByDash[1];

                        int firstValueInteger = ParseVerseToInteger(firstValue);
                        int lastValueInteger = ParseVerseToInteger(lastValue);

                        for (var v = firstValueInteger + 1; v < lastValueInteger; v++)
                            listOfVerses.Add(v.ToString());

                        listOfVerses.Add(lastValue);

                        return listOfVerses;
                    });
        }

        /// <summary>
        ///     When populating the list of verses, if there are no verses specified, interprets the chapter as a verse if the Bible book only has one chapter or populates the list will all verses in a chapter if the Bible book has more than one chapter.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void EvaluateEmptyVerseField()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Converts verse number strings into integers.
        /// </summary>
        /// <param name="verse"></param>
        /// <returns>
        ///     Integer representation of verse numbers inputted as strings.
        /// </returns>
        private static int ParseVerseToInteger(string verse)
            => int.Parse(Regex.Replace(verse, @"[a-z]", "").Trim());

        /// <summary>
        ///     * HELPER FUNCTION *
        /// </summary>
        /// <param name="bibleBookKey"></param>
        /// <returns>
        ///     True if specified Bible book is only one chapter; false otherwise.
        /// </returns>
        public static bool IsOneChapter(string bibleBookKey)
            => OneChapterBooks.Contains(bibleBookKey);
    }
}
