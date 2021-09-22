﻿using System;
using BibleStudyTool.Core.Entities;
using BibleStudyTool.Core.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BibleStudyTool.Infrastructure.Data.Config
{
    public class BibleBookAbbreviationLanguageEntityTypeConfiguration : IEntityTypeConfiguration<BibleBookAbbreviationLanguage>
    {
        public void Configure(EntityTypeBuilder<BibleBookAbbreviationLanguage> builder)
        {
            builder.HasKey(bibleBookAbbreviationLanguage => new
            {
                bibleBookAbbreviationLanguage.BibleBookId,
                bibleBookAbbreviationLanguage.LanguageCode
            });

            builder.Property(bibleBookAbbreviationLanguage => bibleBookAbbreviationLanguage.Abbreviation)
                   .HasColumnName("BibleBookAbbreviation")
                   .IsRequired();

            builder.HasOne<BibleBook>(bibleBookAbbreviationLanguage => bibleBookAbbreviationLanguage.BibleBook)
                   .WithMany(bibleBook => bibleBook.BibleBookAbbreviationLanguages)
                   .HasForeignKey(bibleVerseBibleVersionLanguage => bibleVerseBibleVersionLanguage.BibleBookId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne<Language>(bibleBookAbbreviationLanguage => bibleBookAbbreviationLanguage.Language)
                   .WithMany(language => language.BibleBookAbbreviationLanguages)
                   .HasForeignKey(bibleBookAbbreviationLanguage => bibleBookAbbreviationLanguage.LanguageCode)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
