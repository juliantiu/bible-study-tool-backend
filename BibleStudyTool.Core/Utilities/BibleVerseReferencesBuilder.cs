using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Utilities
{
    public static class BibleVerseReferencesBuilder
    {
        public static IEnumerable<(string, string, string)>
            BuildVerseReferences
                (IEnumerable<(string, string, string)> verseReferences)
        {
            foreach (
                (string bookName, string chapter, string verse)
                in verseReferences)
            {
                // Normalize book name to Bible book key

                // 
            }

            return [];
        }
    }
}
