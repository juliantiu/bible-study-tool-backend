using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferenceParsing
{
    internal class UnchainedBibleVerseReferencesGrouper
    {
        private List<(string bookComponent,
              string chapterComponent,
              string verseComponent)> UnchainedBibleVerseReference;

        internal UnchainedBibleVerseReferencesGrouper
            (List<(string bookComponent,
                   string chapterComponent,
                   string verseComponent)> unchainedBibleVerseReference)
        {
            UnchainedBibleVerseReference = unchainedBibleVerseReference;
        }

        private void GroupByBook()
        {

        }
    }
}
