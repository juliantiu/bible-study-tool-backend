﻿using System;
namespace BibleStudyTool.Public.TagEnpoints
{
    public class UpdateTagResponse
    {
        public bool Success { get; set; } = false;
        public string FailureMessage { get; set; } = string.Empty;

        public UpdateTagResponse()
        {
        }
    }
}
