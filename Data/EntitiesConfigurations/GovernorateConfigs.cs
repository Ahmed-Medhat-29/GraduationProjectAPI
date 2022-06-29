using GraduationProjectAPI.Models.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class GovernorateConfigs : IEntityTypeConfiguration<Governorate>
	{
		public void Configure(EntityTypeBuilder<Governorate> builder)
		{
			builder.HasIndex(g => g.Name).IsUnique();
			builder.HasIndex(g => g.Name_AR).IsUnique();
		}
	}
}
