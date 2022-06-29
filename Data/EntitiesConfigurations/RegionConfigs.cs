using GraduationProjectAPI.Models.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class RegionConfigs : IEntityTypeConfiguration<Region>
	{
		public void Configure(EntityTypeBuilder<Region> builder)
		{
			builder.HasIndex(g => new { g.Name, g.CityId }).IsUnique();
			builder.HasIndex(g => new { g.Name_AR, g.CityId }).IsUnique();
		}
	}
}
