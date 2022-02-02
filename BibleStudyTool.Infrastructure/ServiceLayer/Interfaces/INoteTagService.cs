﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibleStudyTool.Core.Entities;

namespace BibleStudyTool.Infrastructure.ServiceLayer
{
    public interface INoteTagService
    {
        Task BulkCreateNoteTagsAsync(int noteId, IEnumerable<int> tagIds);
        Task<IDictionary<int, IList<Tag>>> GetTagsForNotesAsync(int[] noteIds);
        Task RemoveTagsFromNote(int noteId, IEnumerable<int> tagIdsToDelete);
    }
}