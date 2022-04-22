using GraduationProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraduationProjectAPI.Data.EntitiesConfigurations
{
	public class MediatorConfigs : IEntityTypeConfiguration<Mediator>
	{
		public void Configure(EntityTypeBuilder<Mediator> builder)
		{
			builder.HasIndex(m => m.NationalId).IsUnique();
			builder.HasIndex(m => m.PhoneNumber).IsUnique();

			builder.HasOne(m => m.GeoLocation)
				.WithOne()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.Gender)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.Status)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.SocialStatus)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.Region)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(m => m.Locale)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(m => m.DateRegistered)
				.HasDefaultValueSql("GETDATE()");
		}
	}
}
