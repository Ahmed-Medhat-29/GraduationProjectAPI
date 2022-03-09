using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class StatusConfigs : IEntityTypeConfiguration<Status>
	{
		public void Configure(EntityTypeBuilder<Status> builder)
		{
			var data = new[]
			{
				new Status { Id = 1, Name = "Pending"},
				new Status { Id = 2, Name = "Accepted"},
				new Status { Id = 3, Name = "Rejected"},
				new Status { Id = 4, Name = "Submitted"}
			};

			builder.HasData(data);
		}
	}
}
