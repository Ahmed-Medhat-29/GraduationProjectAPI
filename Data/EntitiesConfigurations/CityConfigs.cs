using GraduationProjectAPI.Models.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class CityConfigs : IEntityTypeConfiguration<City>
	{
		public void Configure(EntityTypeBuilder<City> builder)
		{
			builder.HasIndex(g => new { g.Name, g.GovernorateId }).IsUnique();
			builder.HasIndex(g => new { g.Name_AR, g.GovernorateId }).IsUnique();
		}
	}
}
