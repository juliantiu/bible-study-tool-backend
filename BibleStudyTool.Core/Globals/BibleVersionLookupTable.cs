using BibleStudyTool.Core.Entities.BibleVersionInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Globals
{
    public static class BibleVersionLookupTable
    {
        private static Dictionary <(string language, string versionAbbreviation), BibleVersionInformation> BibleVersionsBank
            = new Dictionary<(string language, string versionAbbreviation), BibleVersionInformation> ();

        /// <summary>
        ///     Populates the bank of Bible version metadata.
        /// </summary>
        /// <param name="bibleVersion"></param>
        public static void PopulateBibleVersionBank(BibleVersionInformation bibleVersion)
        {
            BibleVersionsBank.Add((bibleVersion.Language, bibleVersion.VersionAbbreviation), bibleVersion);
        }

        /// <summary>
        ///     Retrieves a Bible version metadata.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="versionAbbreviation"></param>
        /// <returns>
        ///     A BibleVersionInformation instance.
        /// </returns>
        public static BibleVersionInformation? BibleVersionInformationRequest(string language, string versionAbbreviation)
        {
            return BibleVersionsBank.GetValueOrDefault((language, versionAbbreviation));
        }
    }
}
