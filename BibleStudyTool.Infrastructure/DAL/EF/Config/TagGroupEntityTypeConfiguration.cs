﻿using System;
using BibleStudyTool.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibleStudyTool.Infrastructure.DAL.EF.Config
{
    public class TagGroupEntityTypeConfiguration : IEntityTypeConfiguration<TagGroup>
    {
        public void Configure(EntityTypeBuilder<TagGroup> builder)
        {
            builder.HasKey(tagGroup => tagGroup.Id);

            builder.Property(tagGroup => tagGroup.Uid)
                   .IsRequired();
        }
    }
}
