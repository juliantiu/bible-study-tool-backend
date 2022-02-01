﻿using System.Collections.Generic;

namespace BibleStudyTool.Public.DTOs
{
    public class TagGroupDto
    {
        public IList<TagDto> Tags { get; set; }
        public int TagGroupId { get; set; }
        public string Uid { get; set; }
    }
}
