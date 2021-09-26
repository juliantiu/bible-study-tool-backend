﻿using System;
using System.Threading.Tasks;
using BibleStudyTool.Core.Entities;
using BibleStudyTool.Core.Interfaces;
using BibleStudyTool.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.TagGroupEndpoints
{
    public class Create : ControllerBase
    {
        private readonly IAsyncRepository<TagGroup> _itemRepository;
        private readonly UserManager<BibleReader> _userManager;

        public Create(IAsyncRepository<TagGroup> itemRepository,
                      UserManager<BibleReader> userManager)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
        }

        [HttpPost("api/TagGroup")]
        [Authorize]
        public async Task<ActionResult<CreateTagGroupResponse>> CreateHandler(CreateTagGroupRequest request)
        {
            try
            {
                var response = new CreateTagGroupResponse();
                var tagGroup = new TagGroup(_userManager.GetUserId(User), request.Label);
                await _itemRepository.CreateAsync<TagGroupCrudActionException>(tagGroup);
                response.Success = true;
                return Ok(response);
            }
            catch (TagGroupCrudActionException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new EntityCrudActionExceptionResponse() { Timestamp = ex.Timestamp, Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to create tag group '{request.Label}.'");
            }
        }
    }
}
