using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Entities.BibleVersionInformation
{
    public class BibleVersionInformation
    {
        public string Language { get; private set; }
        public string LanguageEndonym { get; private set; }
        public string LanguageExonym { get; private set; }
        public string VersionAbbreviation { get; private set; }
        public string VersionFullName { get; private set; }

        public Dictionary
            <string, 
                (
                string BookKey,
                string BookName,
                string BookAbbreviation,
                string Testament,
                string Section,
                string Subsection,
                string ChapterCount,
                string VerseCount
                )>
            BibleBookDefinitions { get; private set; }

        public BibleVersionInformation
            (string language,
            string languageEndonym,
            string languageExonym,
            string versionAbbreviation,
            string versionFullName
            )
        {
            Language = language;
            LanguageEndonym = languageEndonym;
            LanguageExonym = languageExonym;
            VersionAbbreviation = versionAbbreviation;
            VersionFullName = versionFullName;
            BibleBookDefinitions =
                new Dictionary
                    <string,
                    (
                        string BookKey,
                        string BookName,
                        string BookAbbreviation,
                        string Testament,
                        string Section,
                        string Subsection,
                        string ChapterCount,
                        string VerseCount
                    )>();
        }
    }
}
