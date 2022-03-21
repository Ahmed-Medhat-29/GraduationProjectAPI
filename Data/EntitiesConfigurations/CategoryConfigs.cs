using GraduationProjectAPI.Models.CaseProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class CategoryConfigs : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			var data = new[]
			{
				new Category { Id = 1, Name = "Medical"},
				new Category { Id = 2, Name = "Poverty"}
			};

			builder.HasData(data);
		}
	}
}
