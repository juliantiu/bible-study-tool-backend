using System.Text.RegularExpressions;
using static BibleStudyTool.Core.Utilities.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities
{
    public static class BibleVerseReferenceParser
    {
        /// <summary>
        ///     Converts raw string of semicolon-separated verse references into
        ///     list of verse references, which can be used to build a list of 
        ///     Bible verses.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     An enumerable set of tuples that describe Bible verse 
        ///     references.
        /// </returns>
        public static IEnumerable<(string, string, string)>
            ParseRawVerseReferences(string rawVerseReferences)
        {
            MatchCollection separatedVerseRefernces
                = TokenizeVerseReferences(rawVerseReferences);

            return GenerateVerseReferenceUnits(separatedVerseRefernces);
        }

        /// <summary>
        ///     * HELPER FUNCTION *
        ///     Separates semicolon-separated verse references into verse 
        ///     reference units.
        /// </summary>
        /// <param name="rawVerseReferences"></param>
        /// <returns>
        ///     A MatchCollection of string verse references.
        /// </returns>
        private static MatchCollection
            TokenizeVerseReferences(string rawVerseReferences)
        {
            string pattern
                = @"((?:[123]\s)?[a-zA-Z]+\.?)?(\s\d+-?\d*:?(?!\s\w*))(\d+[a-z]?-?\d*(?:,)?\s?)*";
            
            RegexOptions regexOptions = RegexOptions.IgnoreCase;

            Regex regex =
                new(pattern, regexOptions, TimeSpan.FromSeconds(1));

            return regex.Matches(rawVerseReferences);
        }

        /// <summary>
        ///     * HELPER FUNCTION *
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
        private static IEnumerable<(string, string, string)>
            GenerateVerseReferenceUnits
                (MatchCollection separatedVerseReferences)
        {
            List<(string, string, string)> parsedVerses = new();


            foreach (Match separatedVerseReference in separatedVerseReferences)
            {
                GroupCollection separatedVerseGroupings
                    = separatedVerseReference.Groups;

                string bookName =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Book]
                            .Value;

                string chapters =
                        separatedVerseGroupings
                            [(int)ParseMatchGroupPosition.Chapter]
                                .Value;

                CaptureCollection potentialVerses =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Verses]
                            .Captures;

                if (HasNoVersesComponent(potentialVerses))
                {
                    parsedVerses
                        .Add(
                            (bookName,
                            chapters.ToString().Trim(':'),
                            string.Empty));

                    continue;
                }

                PopulateVersesComponent
                    (bookName, chapters, potentialVerses, ref parsedVerses);
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
        private static void
            PopulateVersesComponent
                (string bookName,
                string chapters,
                CaptureCollection verses,
                ref List<(string, string, string)> parsedVerses)
        {
            foreach (Capture verse in verses)
                parsedVerses.Add((bookName, chapters, verse.Value.Trim(',')));
        }
    }
}
