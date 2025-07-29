using System.Collections.Generic;
using System.Text.RegularExpressions;
using static BibleStudyTool.Core.Utilities.BibleVerseReferences.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferences
{
    public static class BibleVerseReferenceParser
    {
        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Separates semicolon-separated verse references into verse 
        ///     reference units.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     A MatchCollection of string verse references.
        /// </returns>
        internal static MatchCollection
            TokenizeRawVerseReferenceInputs(string rawVerseReferences)
        {
            string pattern
                = @"((?:[123]\s)?[a-zA-Z]+\.?)?(\s\d+-?\d*:?(?!\s\w*))(\d+[a-z]?-?\d*(?:,)?\s?)*";
            
            RegexOptions regexOptions = RegexOptions.IgnoreCase;

            Regex regex =
                new(pattern, regexOptions, TimeSpan.FromSeconds(1));

            return regex.Matches(rawVerseReferences);
        }

        /// <summary>
        ///     Organizes a set of raw string semicolon-separated verse 
        ///     references into an iterable set of tuples that describe
        ///     Bible verse references in its various components.
        /// </summary>
        /// <param name="separatedVerseReferences"></param>
        /// <returns>
        ///     An enumerable set of tuples that describe Bible verse references 
        ///     by (BookKey, ChapterNumber, VerseNumber).
        /// </returns>
        /// <exception cref="Exception"></exception>
        internal static IEnumerable<(string, string, string)>
            GenerateVerseReferenceUnits
                (MatchCollection separatedVerseReferences)
        {
            List<(string, string, string)> parsedVerses = new();

            string bookName = string.Empty;

            foreach (Match separatedVerseReference in separatedVerseReferences)
            {
                GroupCollection separatedVerseGroupings
                    = separatedVerseReference.Groups;

                string matchedBookName =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Book]
                            .Value;

                bookName = NormalizeBookName(bookName, matchedBookName);

                string chapters =
                        separatedVerseGroupings
                            [(int)ParseMatchGroupPosition.Chapter]
                                .Value
                                    .Trim(':');

                CaptureCollection potentialVerses =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Verses]
                            .Captures;

                if (HasNoVersesComponent(potentialVerses))
                {
                    parsedVerses
                        .Add((bookName,
                            chapters,
                            string.Empty));

                    continue;
                }

                parsedVerses.AddRange
                    (PopulateVersesComponent
                        (bookName, chapters, potentialVerses));
            }

            return parsedVerses;
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Checks if the verse reference has a verse component.
        /// </summary>
        /// <param name="bookname"></param>
        /// <param name="chapters"></param>
        /// <param name="verses"></param>
        /// <param name="parsedVerses"></param>
        private static bool HasNoVersesComponent
            (CaptureCollection verses) => verses.Count() == 0;

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Adds the verse component to the list of verse references tuples.
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="chapters"></param>
        /// <param name="verses"></param>
        /// <param name="parsedVerses"></param>
        /// <returns></returns>
        private static List<(string, string, string)>
            PopulateVersesComponent
                (string bookName,
                string chapters,
                CaptureCollection verses)
        {
            List <(string, string, string)> parsedVerses = new();

            foreach (Capture verse in verses)
                parsedVerses.Add((bookName, chapters, verse.Value.Trim(',')));

            return parsedVerses;
        }
    }
}
