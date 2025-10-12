using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing
{
    internal class BibleVerseReferenceUnit
    {
        internal string BookComponent { get; set; }
        internal string ChapterComponent { get; set; }
        internal string VerseComponent { get; set; }
        internal string StartingChapter { get; set; }
        internal string StartingVerse { get; set; }
        internal string EndingChapter { get; set; }
        internal string EndingVerse { get; set; }

        internal BibleVerseReferenceUnit()
        {
            BookComponent = string.Empty;
            ChapterComponent = string.Empty;
            VerseComponent = string.Empty;
            StartingChapter = string.Empty;
            StartingVerse = string.Empty;
            EndingChapter = string.Empty;
            EndingVerse = string.Empty;
        }

        internal BibleVerseReferenceUnit
            (string bookComponent,
             string chapterComponent,
             string verseComponent,
             string startingChapter,
             string startingVerse,
             string endingChapter,
             string endingVerse)
        {
            BookComponent = bookComponent;
            ChapterComponent = chapterComponent;
            VerseComponent = verseComponent;
            StartingChapter = startingChapter;
            StartingVerse = startingVerse;
            EndingChapter = endingChapter;
            EndingVerse = endingVerse;
        }

        internal BibleVerseReferenceUnit
            (string bookComponent,
             string chapterComponent,
             string verseComponent)
        {
            BookComponent = bookComponent;
            ChapterComponent = chapterComponent;
            VerseComponent = verseComponent;

            StartingChapter = string.Empty;
            StartingVerse = string.Empty;
            EndingChapter = string.Empty;
            EndingVerse = string.Empty;
        }
    }
}
