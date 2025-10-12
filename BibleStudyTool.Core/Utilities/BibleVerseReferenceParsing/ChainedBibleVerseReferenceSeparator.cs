using System.Text.RegularExpressions;
using static BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing
{
    internal class ChainedBibleVerseReferenceSeparator
    {

        private MatchCollection IdentifiedVerseReferences;
        internal List<BibleVerseReferenceUnit> UnchainedBibleVerseReference;

        internal ChainedBibleVerseReferenceSeparator
            (MatchCollection identifiedVerseReferences)
        {
            IdentifiedVerseReferences = identifiedVerseReferences;
            UnchainedBibleVerseReference = new();

            UnchainBibleVerseReferences();
        }

        private void UnchainBibleVerseReferences()
        {
            foreach
                (Match identifiedVerseReferenc
                in IdentifiedVerseReferences)
            {
                GroupCollection separatedVerseGroupings
                    = identifiedVerseReferenc.Groups;

                string bookComponent =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Book]
                            .Value;

                string chaptersComponent =
                        separatedVerseGroupings
                            [(int)ParseMatchGroupPosition.Chapter]
                                .Value.Trim(':') ?? string.Empty;

                string versesComponent =
                    separatedVerseGroupings
                        [(int)ParseMatchGroupPosition.Verses]
                            .Value ?? string.Empty;

                UnchainedBibleVerseReference
                    .Add(new BibleVerseReferenceUnit(bookComponent,
                                                     chaptersComponent,
                                                     versesComponent));
            }
        }
    }
}
