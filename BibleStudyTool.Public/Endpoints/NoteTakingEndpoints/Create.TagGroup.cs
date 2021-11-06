﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibleStudyTool.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.NoteTakingEndpoints
{
    public partial class Create
    {
        [HttpPost("tag-group")]
        public async Task<ActionResult<TagGroupWithTags>> CreateTagGroupAsync([FromBody]IEnumerable<int> tagIds)
        {
            try
            {
                string userId = _userManager.GetUserId(User);
                return Ok(await _tagGroupService.CreateTagGroupAsync(userId, tagIds));
            }
            catch (TagGroupCrudActionException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new EntityCrudActionExceptionResponse() { Timestamp = ex.Timestamp, Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to create tag group.");
            }
        }
    }
}