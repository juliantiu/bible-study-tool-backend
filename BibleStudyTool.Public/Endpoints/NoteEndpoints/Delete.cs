﻿using System;
using System.Threading.Tasks;
using BibleStudyTool.Core.Entities;
using BibleStudyTool.Core.Interfaces;
using BibleStudyTool.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.NoteEndpoints
{
    public class Delete : ControllerBase
    {
        private readonly IAsyncRepository<Note> _itemRepository;
        private readonly UserManager<BibleReader> _userManager;

        public Delete(IAsyncRepository<Note> itemRepository,
                      UserManager<BibleReader> userManager)
        {
            _itemRepository = itemRepository;
            _userManager = userManager;
        }

        [HttpDelete("api/tag")]
        [Authorize]
        public async Task<ActionResult<DeleteNoteResponse>> DeleteHandler(string id)
        {
            try
            {
                var response = new DeleteNoteResponse();
                var currentUserId = _userManager.GetUserId(User);
                var note = await _itemRepository.GetByIdAsync<NoteCrudActionException>(id);
                if (note.Uid != currentUserId)
                {
                    response.FailureMessage = "The current user does not own the note being deleted.";
                    return response;
                }
                await _itemRepository.DeleteAsync<NoteCrudActionException>(note);
                response.Success = true;
                return response;

            }
            catch (NoteCrudActionException ncaex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new EntityCrudActionExceptionResponse() { Timestamp = ncaex.Timestamp, Message = ncaex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete note.");
            }
        }
    }
}
