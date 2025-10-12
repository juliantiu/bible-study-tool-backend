using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing
{
    internal class UnchainedBibleVerseReferencesExpander
    {
        internal List<BibleVerseReferenceUnit> RawBibleVerseReferenceUnits;
        internal List<BibleVerseReferenceUnit> ExpandedBibleVerseReferenceUnits;

        internal UnchainedBibleVerseReferencesExpander
            (List<BibleVerseReferenceUnit> bibleVerseReferenceUnits)
        {
            RawBibleVerseReferenceUnits = bibleVerseReferenceUnits;
            ExpandedBibleVerseReferenceUnits = new();

            ExecuteExpansionProcedure();
        }

        private static readonly Regex SimpleChapterRangeRegex
            = new(@"\b(?<!:)(\d{1,3}):?(\d{1,3}[a-zA-Z]?)?(?:-+|—)(\d{1,3})",
                  RegexOptions.IgnoreCase | RegexOptions.Compiled,
                  TimeSpan.FromSeconds(1));

        private static readonly Regex ComplexChapterRangeRegex
            = new(@"",
                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                TimeSpan.FromSeconds(1));

        private enum ChapterRangeComponent
        {
            FullMatch = 0,
            StartChapter = 1,
            StartVerse = 2,
            EndChapter = 3
        }

        private enum VerseRangeComponent
        {
            StartVerse = 0,
            EndVerse = 1
        }

        private void ExecuteExpansionProcedure()
        {

            string latestBook = string.Empty; 

            foreach (BibleVerseReferenceUnit currentVerseReferenceUnit
                        in RawBibleVerseReferenceUnits)
            {
                BibleVerseReferenceUnit newBibleVerseReferenceUnit
                    = new();

                if (!string.IsNullOrWhiteSpace
                    (currentVerseReferenceUnit.ChapterComponent))
                {

                    latestBook
                        = DetermineCurrentBookName
                            (latestBook,
                             currentVerseReferenceUnit.BookComponent);

                    newBibleVerseReferenceUnit.BookComponent = latestBook;

                    if (SimpleChapterRangeRegex.IsMatch
                            (currentVerseReferenceUnit.ChapterComponent))
                    {
                        ExpandChapterRanges
                            (currentVerseReferenceUnit,
                             newBibleVerseReferenceUnit);
                    }
                    else
                    {
                        newBibleVerseReferenceUnit.ChapterComponent
                            = newBibleVerseReferenceUnit.StartingChapter 
                            = newBibleVerseReferenceUnit.EndingChapter 
                            = currentVerseReferenceUnit.ChapterComponent;

                        if (string.IsNullOrWhiteSpace
                                (currentVerseReferenceUnit.VerseComponent))
                        {
                            newBibleVerseReferenceUnit.StartingVerse = "1";

                            ExpandedBibleVerseReferenceUnits
                                .Add(newBibleVerseReferenceUnit);

                            continue;
                        }
                    }

                }
                if (!string.IsNullOrWhiteSpace
                            (currentVerseReferenceUnit.VerseComponent))
                {
                    ExpandVerses
                        (currentVerseReferenceUnit, newBibleVerseReferenceUnit);
                }
            }
        }

        private string DetermineCurrentBookName
            (string latestBook, string currentBook)
        {
            return String.IsNullOrWhiteSpace(currentBook)
                || String.IsNullOrEmpty(currentBook)
                ? latestBook
                : currentBook;
        }

        private void ExpandChapterRanges
            (BibleVerseReferenceUnit currentBibleVerseReferenceUnit,
             BibleVerseReferenceUnit newBibleVerseReferenceUnit)
        {

            GroupCollection chapterRangeMatches
                = SimpleChapterRangeRegex.Match
                    (currentBibleVerseReferenceUnit.ChapterComponent)
                        .Groups;

            newBibleVerseReferenceUnit.StartingChapter
                = chapterRangeMatches
                    [(int)ChapterRangeComponent.StartChapter]
                        .Value;

            string startingVerse =
                chapterRangeMatches
                    [(int)ChapterRangeComponent.StartVerse]
                        .Value;

            newBibleVerseReferenceUnit.StartingVerse
                = String.IsNullOrWhiteSpace(startingVerse)
                    ? "1" 
                    : startingVerse;

            newBibleVerseReferenceUnit.EndingChapter 
                = chapterRangeMatches
                    [(int)ChapterRangeComponent.EndChapter]
                        .Value;

            if (string.IsNullOrWhiteSpace
                    (currentBibleVerseReferenceUnit.VerseComponent))
            {
                ExpandedBibleVerseReferenceUnits
                    .Add(newBibleVerseReferenceUnit);
            }
        }

        private void ExpandVerses
            (BibleVerseReferenceUnit currentBibleVerseReferenceUnit,
             BibleVerseReferenceUnit newBibleVerseReferenceUnit)
        {
            string[] verses
                = currentBibleVerseReferenceUnit.VerseComponent
                    .Split(new char[] { ',' },
                           StringSplitOptions
                                .RemoveEmptyEntries);

            for (int i = 0; i < verses.Length; i++)
            {
                if (verses[i].Contains("-"))
                {
                    BibleVerseReferenceUnit splitVerseReferenceUnit
                        = new();

                    splitVerseReferenceUnit.BookComponent
                        = newBibleVerseReferenceUnit.BookComponent;

                    string[] verseRangeComponents
                        = verses[i]
                            .Split(new char[] { '-' },
                                    StringSplitOptions.RemoveEmptyEntries);

                    splitVerseReferenceUnit.StartingChapter
                        = newBibleVerseReferenceUnit.StartingChapter;

                    splitVerseReferenceUnit.EndingChapter
                        = newBibleVerseReferenceUnit.EndingChapter;

                    if (i > 0)
                    {
                        splitVerseReferenceUnit.StartingChapter
                            = splitVerseReferenceUnit.EndingChapter
                            = newBibleVerseReferenceUnit.EndingChapter;
                    }

                    splitVerseReferenceUnit.VerseComponent
                        = verses[i];

                    splitVerseReferenceUnit.StartingVerse
                        = verseRangeComponents
                            [(int)VerseRangeComponent.StartVerse].Trim();

                    splitVerseReferenceUnit.EndingVerse
                        = verseRangeComponents
                            [(int)VerseRangeComponent.EndVerse].Trim();

                    ExpandedBibleVerseReferenceUnits
                        .Add(splitVerseReferenceUnit);

                    continue;
                }

                string trimmedVerse = verses[i].Trim();

                if (i == 0)
                {
                    newBibleVerseReferenceUnit.VerseComponent
                        = newBibleVerseReferenceUnit.EndingVerse
                        = trimmedVerse;

                    if (string.IsNullOrWhiteSpace
                        (newBibleVerseReferenceUnit.StartingVerse))
                    {
                        newBibleVerseReferenceUnit.StartingVerse
                            = trimmedVerse;
                    }

                    ExpandedBibleVerseReferenceUnits
                        .Add(newBibleVerseReferenceUnit);
                }
                else
                {
                    BibleVerseReferenceUnit additionalVerseReferenceUnit
                        = new(newBibleVerseReferenceUnit.BookComponent,
                              newBibleVerseReferenceUnit.ChapterComponent,
                              trimmedVerse,
                              newBibleVerseReferenceUnit.EndingChapter,
                              trimmedVerse,
                              newBibleVerseReferenceUnit.EndingChapter,
                              trimmedVerse);

                    ExpandedBibleVerseReferenceUnits
                        .Add(additionalVerseReferenceUnit);
                }
            }
        }
    }
}

