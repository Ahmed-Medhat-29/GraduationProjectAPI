using GraduationProjectAPI.Models.CaseProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class RelationshipConfigs : IEntityTypeConfiguration<Relationship>
	{
		public void Configure(EntityTypeBuilder<Relationship> builder)
		{
			var data = new[]
			{
				new Relationship { Id = 1, Name = "Self"},
				new Relationship { Id = 2, Name = "Family"},
				new Relationship { Id = 3, Name = "Neighbor"}
			};

			builder.HasData(data);
		}
	}
}
