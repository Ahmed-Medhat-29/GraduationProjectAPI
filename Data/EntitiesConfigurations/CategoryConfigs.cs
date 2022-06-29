using GraduationProjectAPI.Models.CaseProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class CategoryConfigs : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasIndex(g => g.Name).IsUnique();
			builder.HasIndex(g => g.Name_AR).IsUnique();
		}
	}
}
