using BibleStudyTool.Core.Entities.BibleVerse;
using System.Text.RegularExpressions;
using static BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing
{
    public class ChainedBibleVerseReferencesParser
    {
        private string Language;
        private string BibleVersion;
        private string RawVerseReferencesInput;

        private MatchCollection RawChainedVerseReferences;
        private List<BibleVerseReferenceUnit> RawUnchainedBibleVerseReference;

        private List<BibleVerseReferenceUnit> BibleVerseReferenceUnits;

        private List<BibleVerse> BibleVerses { get; }

        public ChainedBibleVerseReferencesParser
                (string language,
                string bibleVersion,
                string rawVerseReferencesInput)
        {
            Language = language;
            BibleVersion = bibleVersion;
            RawVerseReferencesInput = rawVerseReferencesInput;
            RawUnchainedBibleVerseReference = new();
            BibleVerseReferenceUnits = new();

            IdentifyBibleVerseReferencesFromRawInput();
            SeparateBibleVerseReferenceChain();
            ExpandUnchainedBibleVerseReferences();
        }

        /// <summary>
        /// 
        /// </summary>
        private void IdentifyBibleVerseReferencesFromRawInput()
        {

            string ParsingRegexPattern
                = @"\b((?<!,|-{1,}|—)[1-3]?\s?[A-Za-z]+\.?)?\s*((?<!,\s*)\d{1,3}(?::\d*(?:-{2,})?(?:—)?\d{1,3}:)?(?:-\d)*)\s*(?::)?\s*(\d{1,3}[a-z]?(?:[,|-]\s*\d{1,3}[a-z]?)*)?";
            
            Regex rgx
                = new(ParsingRegexPattern,
                      RegexOptions.IgnoreCase,
                      TimeSpan.FromSeconds(1));

            RawChainedVerseReferences
                = rgx
                    .Matches(RawVerseReferencesInput);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SeparateBibleVerseReferenceChain()
        {
            ChainedBibleVerseReferenceSeparator separator
                = new(RawChainedVerseReferences);

            RawUnchainedBibleVerseReference
                = separator.UnchainedBibleVerseReference;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExpandUnchainedBibleVerseReferences()
        {
            UnchainedBibleVerseReferencesExpander expander
                = new(RawUnchainedBibleVerseReference);

            BibleVerseReferenceUnits
                = expander.ExpandedBibleVerseReferenceUnits;
        }

    }
}
