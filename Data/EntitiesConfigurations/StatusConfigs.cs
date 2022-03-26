using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaticStrings = GraduationProjectAPI.Utilities.StaticStrings;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class StatusConfigs : IEntityTypeConfiguration<Status>
	{
		public void Configure(EntityTypeBuilder<Status> builder)
		{
			var data = new[]
			{
				new Status { Id = 1, Name = StaticStrings.Status.Pending},
				new Status { Id = 2, Name = StaticStrings.Status.Accepted},
				new Status { Id = 3, Name = StaticStrings.Status.Rejected},
				new Status { Id = 4, Name = StaticStrings.Status.Submitted}
			};

			builder.HasData(data);
		}
	}
}
