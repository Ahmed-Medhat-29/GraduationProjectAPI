using GraduationProjectAPI.Models;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Shared;
using GraduationProjectAPI.Utilities.StaticStrings;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectAPI.Data
{
	public static class DataSeeding
	{
		public static void Seed(ModelBuilder builder)
		{
			builder.Entity<Gender>().HasData(StaticValues.Genders());
			builder.Entity<Status>().HasData(StaticValues.Status());
			builder.Entity<Locale>().HasData(StaticValues.Locales());
			builder.Entity<SocialStatus>().HasData(StaticValues.SocialStatus());
			builder.Entity<Period>().HasData(StaticValues.Periods());
			builder.Entity<Priority>().HasData(StaticValues.Priorities());
			builder.Entity<Relationship>().HasData(StaticValues.Relationships());
			builder.Entity<NotificationType>().HasData(StaticValues.NotificationTypes());
			builder.Entity<MessageType>().HasData(StaticValues.MessageTypes());
		}
	}
}
