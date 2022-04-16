﻿using GraduationProjectAPI.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class MediatorReviewConfigs : IEntityTypeConfiguration<MediatorReview>
	{
		public void Configure(EntityTypeBuilder<MediatorReview> builder)
		{
			builder.HasKey(mr => new { mr.RevieweeId, mr.ReviewerId });

			builder.HasOne(m => m.Reviewee)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.Reviewer)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}