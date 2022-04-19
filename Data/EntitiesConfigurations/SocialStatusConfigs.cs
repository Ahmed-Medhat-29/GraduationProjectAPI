using System;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class SocialStatusConfigs : IEntityTypeConfiguration<SocialStatus>
	{
		public void Configure(EntityTypeBuilder<SocialStatus> builder)
		{
			var data = Enum.GetValues<SocialStatusType>()
				.Select(e => new SocialStatus
				{
					Id = (byte)e,
					Name = e.ToString()
				});

			builder.HasData(data);
		}
	}
}
