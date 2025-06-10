using BibleStudyTool.Core.Entities.BibleVerse;
using BibleStudyTool.Infrastructure.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.BibleVerseEndpoints
{
    [ApiController]
    [Route("api/bible-verses/read")]
    public class Read : ControllerBase
    {

        private readonly IBibleVerseService _bibleVerseService;

        public Read(IBibleVerseService bibleVerseService)
        {
            _bibleVerseService = bibleVerseService;
        }

        [HttpGet(Name = "get-bible-verse-references")]
        public async Task<ActionResult<IEnumerable<BibleVerse>>>
        GetBibleVerseReferences(string language, string versionAbbreviation, string rawVerseReferences)
        {
            try
            {
                return Ok(await _bibleVerseService.SearchVerseReferences(language, versionAbbreviation, rawVerseReferences));
            } catch (Exception ex)
            {
                return StatusCode
                    (StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
