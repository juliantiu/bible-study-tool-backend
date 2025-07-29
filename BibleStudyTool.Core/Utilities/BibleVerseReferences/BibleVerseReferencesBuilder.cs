using System.Collections.Generic;
using static BibleStudyTool.Core.Globals.BibleVersionsDetailsStore;
using static BibleStudyTool.Core.Utilities.BibleVerseReferences.BibleVerseHelper;

namespace BibleStudyTool.Core.Utilities.BibleVerseReferences
{
    /// <summary>
    /// 
    /// </summary>
    public static class BibleVerseReferencesBuilder
    {

        /// <summary>
        /// 
        /// </summary>
        internal enum Component
        {
            Chapters = 1,
            Verses = 2
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chaptersComponent"></param>
        /// <returns></returns>
        internal static List<(string, string, string)>
            PopulateOneChapterVerses
                (string language,
                string version,
                string bookKey,
                string chaptersComponent)
        {
            if (IsValidComponent
                (language,
                version,
                bookKey,
                chaptersComponent,
                Component.Chapters))
            {
                return new List<(string, string, string)>
                {
                    (bookKey, "1", chaptersComponent)
                };
            }
            return [];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chaptersComponent"></param>
        /// <returns></returns>
        internal static List<(string, string, string)>
            PopulateChapterRangeVerses
                (string language,
                string version,
                string bookKey,
                string chaptersComponent)
        {

            int[] chaptersRange = AccquireComponentRange(chaptersComponent);

            List<string> chaptersInRange =
                InterpolateComponentRange
                    (language,
                    version,
                    bookKey,
                    chaptersRange,
                    Component.Chapters);

            return chaptersInRange
                .Aggregate
                    (new List<(string, string, string)>(),
                    (verseReferences, chapter) =>
                    {
                        verseReferences
                            .AddRange
                                (PopulateWholeChapterVerses
                                    (language,
                                    version,
                                    bookKey,
                                    chapter));

                        return verseReferences;
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static int[] AccquireComponentRange(string component)
        {
            string[] splitByDash = component.Split('-');

            try
            {
                int firstValue = NumerizeNumericComponent(splitByDash[0]);

                if (splitByDash.Length <= 1 && firstValue > 0)
                    return [firstValue, firstValue];

                int lastValue = NumerizeNumericComponent(splitByDash[1]);

                if (lastValue > firstValue)
                    return [firstValue, lastValue];
            }
            catch
            {
                // TODO: Log error
            }


            return [0, 0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="componentRange"></param>
        /// <returns></returns>
        internal static List<string>
            InterpolateComponentRange
                (string language,
                string version,
                string bookKey,
                int[] componentRange,
                Component component,
                string verseChapter = "0")
        {
            int startValue = componentRange[0];
            int endValue = componentRange[1];

            if (!AreValidComponents
                    (language,
                    version,
                    bookKey,
                    [$"{startValue}", $"{endValue}"],
                    component,
                    verseChapter))
                return [];

            if (endValue < startValue) return [];

            List<string> interpolatedComponent = new();

            for (var c = startValue; c <= endValue; c++)
                interpolatedComponent.Add($"{c}");

            return interpolatedComponent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chapters"></param>
        /// <returns></returns>
        internal static bool
            AreValidComponents
                (string language,
                string version,
                string bookKey,
                string[] components,
                Component componentType,
                string verseChapter = "0")
        {
            foreach (string component in components)
            {
                if (!IsValidComponent
                        (language,
                        version,
                        bookKey,
                        component,
                        componentType,
                        verseChapter))

                    return false;
            }

            return true;
        }

        internal static bool
           IsValidComponent
                (string language,
                string version,
                string bookKey,
                string component,
                Component componentType,
                string verseChapter = "0")
        {

            switch (componentType)
            {
                case Component.Chapters:

                    if (IsChapterInBook
                            (language,
                            version,
                            bookKey,
                            component))
                        return true;
                    break;

                case Component.Verses:

                    int totalVersesInChaper = GetTotalVersesInChapter
                        (language, version, bookKey, verseChapter);

                    int componentValue = NumerizeNumericComponent(component);

                    if (componentValue > 0
                        && componentValue <= totalVersesInChaper)
                        
                        return true;

                    break;

                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versesComponent"></param>
        /// <returns></returns>
        internal static bool NoVerseComponent
            (string versesComponent) => string.IsNullOrEmpty(versesComponent);

        internal static List<(string, string, string)>
            PopulateWholeChapterVerses
                (string language,
                string version,
                string bookKey,
                string chapter)
        {
            List<(string, string, string)> verseReferences = new();
            int totalVerses =
                GetTotalVersesInChapter
                    (language, version, bookKey, $"{chapter}");

            for (int verse = 1; verse <= totalVerses; verse++)
            {
                verseReferences.Add((bookKey, $"{chapter}", $"{verse}"));
            }

            return verseReferences;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="bookKey"></param>
        /// <param name="chaptersComponent"></param>
        /// <param name="versesComponent"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static IEnumerable<(string, string, string)>
            PopulateBibleVerseRange
                (string language,
                string version, 
                string bookKey, 
                string chaptersComponent, 
                string versesComponent)
        {

            List<(string, string, string)> verseReferences = new();

            // Check for case 3 John 1:2
            // 3 John 2:2 should be invalid

            int[] verseRange = AccquireComponentRange(versesComponent);

            if(IsValidComponent
                (language,
                version,
                bookKey,
                chaptersComponent,
                Component.Chapters))
            {
                List<string> versesInRange =
                    InterpolateComponentRange
                        (language,
                        version,
                        bookKey,
                        verseRange,
                        Component.Verses,
                        chaptersComponent);

                return versesInRange
                    .Aggregate
                        (new List<(string, string, string)>(),
                        (verseReferences, verse) =>
                        {
                            verseReferences
                                .Add((bookKey, chaptersComponent, verse));

                            return verseReferences;
                        }); ;
            }

            return verseReferences;

        }
    }
}
