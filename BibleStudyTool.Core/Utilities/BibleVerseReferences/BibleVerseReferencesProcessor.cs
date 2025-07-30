using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Core.Globals;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferences
{
    public class BibleVerseReferencesProcessor
    {
        private string Language;
        private string BibleVersion;

        private List<(string bookKey,
                      string chaptersComponent,
                      string versesComponent)>
            PhaseOneVerseReferenceUnits { get; set; }

        private List<(string bookKey,
              string chaptersComponent,
              string versesComponent)>
            PhaseTwoVerseReferenceUnits
                { get; set; }

        private string RawVerseReferencesInput { get; set; }

        private List<BibleVerse> BibleVerses { get; set; }

        public BibleVerseReferencesProcessor
            (string language,
            string bibleVersion,
            string rawVerseReferencesInput)
        {
            Language = language;
            BibleVersion = bibleVersion;
            RawVerseReferencesInput = rawVerseReferencesInput;

            BibleVerses = new List<BibleVerse>();
            PhaseOneVerseReferenceUnits = new List<(string, string, string)>();
            PhaseTwoVerseReferenceUnits = new List<(string, string, string)>();
        }

        public List<BibleVerse>
            ParseBibleVerseReferencesInput(string rawVerseReferencesInput)
        {

            RawVerseReferencesInput = rawVerseReferencesInput;

            PhaseOneProcessing();
            PhaseTwoProcessing();
            PhaseThreeProcessing();

            return BibleVerses;
        }

        private void PhaseOneProcessing()
        {
            // Separates the raw verse references input by semicolon
            // and puts them in a list of tuples in the structure of
            //  (string bookKey,
            //  string chaptersComponent,
            //  string versesComponent)

            MatchCollection TokenizedRawVerseRefences =
                BibleVerseReferenceParser
                    .TokenizeRawVerseReferenceInputs(RawVerseReferencesInput);

            PhaseOneVerseReferenceUnits
                .AddRange
                    (BibleVerseReferenceParser
                        .GenerateVerseReferenceUnits
                        (TokenizedRawVerseRefences));
        }

        private void PhaseTwoProcessing()
        {
            // Expands the list of tuples from phase one by determining:
            // (1) if verse reference has no verses
            //     -- Is it a whole chapter reference?
            //     -- Is it a range of chapters?
            //     -- Does the chapter onle have one chapter?
            // (2) if verse reference has a range of verses

            string currentBookKey = String.Empty;

            foreach ((string, string, string) verseReferenceUnit
                    in PhaseOneVerseReferenceUnits)
            {

                string bookKey = verseReferenceUnit.Item1;
                string chaptersComponent = verseReferenceUnit.Item2;
                string versesComponent = verseReferenceUnit.Item3;

                currentBookKey = bookKey ?? currentBookKey;

                if (BibleVerseReferencesBuilder
                        .NoVerseComponent(versesComponent))
                {
                    if (BibleVerseHelper.HasOneChapter(bookKey!))
                    {
                        // case : 3 John 2
                        PhaseTwoVerseReferenceUnits
                            .AddRange
                                (BibleVerseReferencesBuilder
                                    .PopulateOneChapterVerses
                                        (Language,
                                        BibleVersion,
                                        currentBookKey,
                                        chaptersComponent));
                    }
                    else
                    {
                        // case : "Genesis 1-3 or Genesis 1"
                        PhaseTwoVerseReferenceUnits
                            .AddRange
                                (BibleVerseReferencesBuilder
                                    .PopulateChapterRangeVerses
                                        (Language,
                                        BibleVersion,
                                        currentBookKey,
                                        chaptersComponent));
                    }
                }
                else
                {
                    // case: "John 1:1-3" or "Matthew 1:1"
                    PhaseTwoVerseReferenceUnits
                        .AddRange
                            (BibleVerseReferencesBuilder
                            .PopulateBibleVerseRange
                                (Language,
                                BibleVersion,
                                currentBookKey,
                                chaptersComponent,
                                versesComponent));
                }
            }
        }

        private void PhaseThreeProcessing()
        {
            // Checks the validity of each verse reference and adds them to the
            // list of BibleVerses.

            string currentBookKey = String.Empty;
            
            foreach (var verseUnits in PhaseTwoVerseReferenceUnits)
            {
                string bookKey = verseUnits.bookKey;
                string chapterComponent = verseUnits.chaptersComponent;
                string verseComponent = verseUnits.versesComponent;

                if (BibleVersionsDetailsStore.IsChapterInBook
                        (Language,
                        BibleVersion,
                        bookKey,
                        chapterComponent))
                {
                    if (BibleVersionsDetailsStore.IsVerseInChapter
                        (Language,
                        BibleVersion,
                        bookKey,
                        chapterComponent,
                        verseComponent))
                    {
                        BibleVerses
                            .Add
                                (new BibleVerse
                                    (Language,
                                    BibleVersion,
                                    verseUnits.bookKey,
                                    int.Parse(verseUnits.chaptersComponent),
                                    int.Parse(verseUnits.versesComponent)));
                    }
                }
            }
        }
    }
}
