using BibleStudyTool.Core.Entities.BibleVersionDetails;
using BibleStudyTool.Core.Globals;
using System.Xml;

namespace BibleStudyTool.Public.HostedServices
{
    public class InitBibleVersionDetailsService : IHostedService
    {
        private static readonly char[] trimChars = new char[] { '_' };

        public Task StartAsync(CancellationToken cancellationToken)
        {

            var coreProjectDirectory =
                Directory.GetParent(Directory.GetCurrentDirectory())?.FullName
                + "\\BibleStudyTool.Core";
            
            var bibleVersionInformationXMLDirectory = 
                coreProjectDirectory + "\\Configs\\BibleVersionInformationXML";

            foreach (string filePath in Directory.GetFiles(bibleVersionInformationXMLDirectory, "*.xml"))
            {
                try
                {
                    XmlDocument xmlDoc = new();
                    xmlDoc.Load(filePath);
                    XmlElement? root = xmlDoc.DocumentElement;

                    if (root == null) continue;

                    string language = GetInnerText(root.SelectSingleNode("Language"));
                    string languageEndonym = GetInnerText(root.SelectSingleNode("LanguageEndonym"));
                    string languageExonym = GetInnerText(root.SelectSingleNode("LanguageExonym"));
                    string versionAbbreviation = GetInnerText(root.SelectSingleNode("VersionAbbreviation"));
                    string versionFullName = GetInnerText(root.SelectSingleNode("VersionFullName"));

                    BibleVersionDetails bibleVersionInformation
                        = new BibleVersionDetails
                            (
                                language,
                                languageEndonym,
                                languageExonym,
                                versionAbbreviation,
                                versionFullName
                            );

                    foreach (XmlNode bibleBook in root.SelectSingleNode("BibleBookDefinitions")!)
                    {
                        string bookKey = bibleBook.Attributes?["bookKey"]?.Value ?? string.Empty;
                        string bookName = GetInnerText(bibleBook.SelectSingleNode("BookName"));
                        string bookAbbreviation = GetInnerText(bibleBook.SelectSingleNode("BookAbbreviation"));
                        string testament = GetInnerText(bibleBook.SelectSingleNode("Testament"));
                        string section = GetInnerText(bibleBook.SelectSingleNode("Section"));
                        string subsection = GetInnerText(bibleBook.SelectSingleNode("Subsection"));
                        int chapterCount = GetInnerTextAsInt(bibleBook.SelectSingleNode("ChapterCount"));
                        int bookOrder = GetInnerTextAsInt(bibleBook.SelectSingleNode("BookOrder"));
                        int verseCountTotal = GetInnerTextAsInt(bibleBook.SelectSingleNode("VerseCountTotal"));

                        Dictionary<int, int> verseCountPerChapter = new();

                        foreach (XmlNode chapter in bibleBook.SelectSingleNode("VerseCountPerChapter")!)
                        {
                            if(int.TryParse(chapter.Name.Trim(trimChars), out var key))
                            {
                                verseCountPerChapter.TryAdd(key, GetInnerTextAsInt(chapter));
                            }

                        }

                        bibleVersionInformation.BibleBookDefinitions.Add
                            (
                                bookKey,
                                (
                                    BookKey: bookKey,
                                    BookName: bookName,
                                    BookAbbreviation: bookAbbreviation,
                                    Testament: testament,
                                    Section: section,
                                    Subsection: subsection,
                                    ChapterCount: chapterCount,
                                    BookOrder: bookOrder,
                                    VerseCountTotal: verseCountTotal,
                                    VerseCountPerChapter: verseCountPerChapter
                                )
                            );

                    }

                    BibleVersionsDetailsStore.AddBibleVersionInformation(bibleVersionInformation);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed.
                    Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                }
            }

            return Task.CompletedTask;
        }

        private string GetInnerText(XmlNode? node)
        {
            return node?.InnerText ?? string.Empty;
        }

        private int GetInnerTextAsInt(XmlNode? node)
        {
            return int.TryParse(node?.InnerText, out var value) ? value : -1;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Cleanup logic if necessary.
            return Task.CompletedTask;
        }
    }
}
