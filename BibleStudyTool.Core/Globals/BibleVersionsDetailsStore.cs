using BibleStudyTool.Core.Entities.BibleVersionDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleStudyTool.Core.Globals
{
    public static class BibleVersionsDetailsStore
    {
        private static readonly Dictionary
            <string, Dictionary<string, BibleVersionDetails>>
                _bibleVersionsInformationStore = new();
        public static void AddBibleVersionInformation(BibleVersionDetails bibleVersionInformation)
        {
            string language = bibleVersionInformation.Language;
            string versionAbbreviation = bibleVersionInformation.VersionAbbreviation;

            if (!_bibleVersionsInformationStore.ContainsKey(language))
            {
                _bibleVersionsInformationStore[language] = new Dictionary<string, BibleVersionDetails>();
            }
            _bibleVersionsInformationStore[language][versionAbbreviation] = bibleVersionInformation;
        }
        public static BibleVersionDetails? GetBibleVersionInformation(string language, string versionAbbreviation)
        {
            if (_bibleVersionsInformationStore.TryGetValue(language, out var versions) &&
                versions.TryGetValue(versionAbbreviation, out var bibleVersionInfo))
            {
                return bibleVersionInfo;
            }
            return null;
        }
    }
}
