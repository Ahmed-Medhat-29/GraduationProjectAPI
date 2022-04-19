using System;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class PeriodConfigs : IEntityTypeConfiguration<Period>
	{
		public void Configure(EntityTypeBuilder<Period> builder)
		{
			var data = Enum.GetValues<PeriodType>()
				.Select(e => new Period
				{
					Id = (byte)e,
					Name = e.ToEnumString()
				});

			builder.HasData(data);
		}
	}
}
