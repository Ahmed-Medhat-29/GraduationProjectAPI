using System;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class RelationshipConfigs : IEntityTypeConfiguration<Relationship>
	{
		public void Configure(EntityTypeBuilder<Relationship> builder)
		{
			var data = Enum.GetValues<RelationshipType>()
				.Select(e => new Relationship
				{
					Id = (byte)e,
					Name = e.ToString()
				});

			builder.HasData(data);
		}
	}
}
