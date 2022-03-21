using GraduationProjectAPI.Models.CaseProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class PriorityConfigs : IEntityTypeConfiguration<Priority>
	{
		public void Configure(EntityTypeBuilder<Priority> builder)
		{
			var data = new[]
			{
				new Priority { Id = 1, Name = "Urgent"},
				new Priority { Id = 2, Name = "High"},
				new Priority { Id = 3, Name = "Normal"}
			};

			builder.HasData(data);
		}
	}
}
