using BibleStudyTool.Core.Interfaces.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Entities.BibleVerse
{
    public class BibleVerse : BaseEntity
    {
        public string BibleVerseId { get; private set; }
        public string Language { get; private set; }
        public string VersionFullName { get; private set; }
        public string VersionAbbreviation { get; private set; }
        public string BookKey { get; private set; }
        public string BookFullName { get; private set; }
        public string BookAbbreviation { get; private set; }
        public string VerseText { get; private set; }
        public int ChapterNumber { get; private set; }
        public int VerseNumber { get; private set; }

        public BibleVerse
            (string bibleVerseId,
            string language,
            string versionFullName,
            string versionAbbreviation,
            string bookKey,
            string bookFullName,
            string bookAbbreviation,
            string verseText,
            int chapterNumber,
            int verseNumber)
        {
            BibleVerseId = bibleVerseId;
            Language = language;
            VersionFullName = versionFullName;
            VersionAbbreviation = versionAbbreviation;
            BookKey = bookKey;
            BookFullName = bookFullName;
            BookAbbreviation = bookAbbreviation;
            VerseText = verseText;
            ChapterNumber = chapterNumber;
            VerseNumber = verseNumber;
        }

        public BibleVerse
            (string language,
            string versionAbbreviation,
            string bookKey,
            int chapterNumber,
            int verseNumber)
        {
            Language = language;
            VersionAbbreviation = versionAbbreviation;
            BookKey = bookKey;
            VerseText = string.Empty;
            ChapterNumber = chapterNumber;
            VerseNumber = verseNumber;
        }

        public void SetBibleVerseId(string newBibleVerseId)
        {
            BibleVerseId = newBibleVerseId;
        }

        public void SetVerseText(string newVerseText)
        {
            VerseText = newVerseText;
        }
    }
}

/*
CREATE TABLE bible_verses (
	bible_verse_id character varying (256) NOT NULL PRIMARY KEY,
	language character (3) NOT NULL,
	version_abbreviation character varying (256) NOT NULL,
	book_key character varying (256) NOT NULL,
	chapter_number smallint NOT NULL,
	verse_number smallint NOT NULL,
	verse_text text NOT NULL,
    search_verse_reference text NOT NULL
);

CREATE INDEX ON bible_verses (language, version_abbreviation, book_key, chapter_number, verse_number);
 * */
