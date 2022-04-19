using System;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class StatusConfigs : IEntityTypeConfiguration<Status>
	{
		public void Configure(EntityTypeBuilder<Status> builder)
		{
			var data = Enum.GetValues<StatusType>()
				.Select(e => new Status
				{
					Id = (byte)e,
					Name = e.ToString()
				});

			builder.HasData(data);
		}
	}
}
