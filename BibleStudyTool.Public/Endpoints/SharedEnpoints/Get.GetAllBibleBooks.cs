﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibleStudyTool.Core.Entities.JoinEntities;
using BibleStudyTool.Core.Entities.JoinEntities.Exceptions;
using BibleStudyTool.Core.Entities.JoinEntities.Specifications;
using BibleStudyTool.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.SharedEnpoints
{
    public partial class Get
    {
        [HttpGet("get-all-bible-books")]
        public async Task<ActionResult<GetAllBibleBooksResponse>> GetAllBibleBooks(string languageCode, string style)
        {
            try
            {
                var response = await GetAllBibleBooks(languageCode, style, _bibleBookAbbreviationLanguageRepository, _bibleBookLanguageRepository);
                return Ok(response);
            }
            catch (BibleBookLanguageCrudActionException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new EntityCrudActionExceptionResponse() { Message = ex.Message, Timestamp = ex.Timestamp });

            }
            catch (BibleBookAbbreviationLanguageCrudActionException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new EntityCrudActionExceptionResponse() { Message = ex.Message, Timestamp = ex.Timestamp });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"Failed to get all bible book names and abbreviations in {languageCode} and {style} style.");
            }
        }

        public static async Task<GetAllBibleBooksResponse>GetAllBibleBooks(string languageCode,
                                                                                               string style,
                                                                                               IAsyncRepository<BibleBookAbbreviationLanguage> _bibleBookAbbreviationLanguageRepository,
                                                                                               IAsyncRepository<BibleBookLanguage> _bibleBookLanguageRepository)
        {
            var response = new GetAllBibleBooksResponse();
            var bibleBookIdNameMapping = new Dictionary<int, string>();
            var bibleBookLanguageSpecRef = new BibleBookLanguage(languageCode, style);
            var bibleBookLanguageSpecification = new BibleBookInLanguageAndStyleSpecification(bibleBookLanguageSpecRef);
            var bibleBookLanguages = await _bibleBookLanguageRepository.GetBySpecification<BibleBookLanguageCrudActionException>(bibleBookLanguageSpecification);
            foreach (var bibleBook in bibleBookLanguages)
                bibleBookIdNameMapping.Add(bibleBook.BibleBookId, bibleBook.Name);

            var bibleBookIdAbbreviationMapping = new Dictionary<int, string>();
            var bibleBookAbbreviationLanguageSpecRef = new BibleBookAbbreviationLanguage(languageCode, style);
            var bibleBookAbbreviationLanguageSpecification = new BibleBookAbbreviationInLanguageAndStyleSpecification(bibleBookAbbreviationLanguageSpecRef);
            var bibleBookAbbreviationLanguages =
                await _bibleBookAbbreviationLanguageRepository
                    .GetBySpecification<BibleBookAbbreviationLanguageCrudActionException>(bibleBookAbbreviationLanguageSpecification);
            foreach (var bibleBook in bibleBookAbbreviationLanguages)
                bibleBookIdAbbreviationMapping.Add(bibleBook.BibleBookId, bibleBook.Abbreviation);

            foreach (var bibleBook in bibleBookIdNameMapping)
            {
                var id = bibleBook.Key;
                var bibleBookName = bibleBook.Value;
                var bibleBookAbbreviation = string.Empty;
                if (bibleBookIdAbbreviationMapping.TryGetValue(id, out string abbreviation))
                    bibleBookAbbreviation = abbreviation;

                response.BibleBookNamesAndAbbreviations.Add(new BibleBookItem()
                {
                    BibleBookAbbreviation = bibleBookAbbreviation,
                    BibleBookId = id,
                    BibleBookName = bibleBookName
                });
            }
            response.Success = true;
            response.LanguageCode = languageCode;
            response.Style = style;

            return response;
        }
    }
}