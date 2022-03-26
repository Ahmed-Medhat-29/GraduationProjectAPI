using GraduationProjectAPI.Models.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class CaseReviewConfigs : IEntityTypeConfiguration<CaseReview>
	{
		public void Configure(EntityTypeBuilder<CaseReview> builder)
		{
			builder.HasKey(cr => new { cr.MediatorId, cr.CaseId });
		}
	}
}
