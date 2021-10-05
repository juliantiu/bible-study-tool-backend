﻿using System;
using BibleStudyTool.Core.Entities;
using BibleStudyTool.Core.Interfaces;
using BibleStudyTool.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibleStudyTool.Public.Endpoints.TagEndpoints
{
    [ApiController]
    public partial class Get : ControllerBase
    {
        private readonly IAsyncRepository<Tag> _tagRepository;
        private readonly UserManager<BibleReader> _userManager;

        public Get(IAsyncRepository<Tag> tagRepository,
                   UserManager<BibleReader> userManager)
        {
            _tagRepository = tagRepository;
            _userManager = userManager;
        }
    }
}
