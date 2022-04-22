using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class NotificationConfigs : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.HasOne(n => n.Type)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
