using GraduationProjectAPI.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class MediatorReviewConfigs : IEntityTypeConfiguration<MediatorReview>
	{
		public void Configure(EntityTypeBuilder<MediatorReview> builder)
		{
			builder.HasKey(mr => new { mr.RevieweeId, mr.ReviewerId });

			builder.HasOne(mr => mr.Reviewee)
				.WithMany(m => m.ReviewsAboutMe)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(mr => mr.Reviewer)
				.WithMany(m => m.ReviewsByMe)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(mr => mr.DateReviewed)
				.HasDefaultValueSql("GETDATE()");
		}
	}
}
