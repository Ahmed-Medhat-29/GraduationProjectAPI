using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class SocialStatusConfigs : IEntityTypeConfiguration<SocialStatus>
	{
		public void Configure(EntityTypeBuilder<SocialStatus> builder)
		{
			var data = new[]
			{
				new Status { Id = 1, Name = "Single"},
				new Status { Id = 2, Name = "Married"},
				new Status { Id = 3, Name = "Divorced"},
				new Status { Id = 4, Name = "Widow"}
			};

			builder.HasData(data);
		}
	}
}
