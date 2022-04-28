using System;
using System.Collections.Generic;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Shared;
using GraduationProjectAPI.Utilities.General;

namespace GraduationProjectAPI.Utilities.StaticStrings
{
	public static class StaticValues
	{
		public static IEnumerable<Period> Periods()
		{
			return Enum.GetValues<PeriodType>()
				.Select(e => new Period { Id = (byte)e, Name = e.ToEnumString() });
		}

		public static IEnumerable<Models.NotificationType> NotificationTypes()
		{
			return Enum.GetValues<NotificationType>()
				.Select(e => new Models.NotificationType { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<Gender> Genders()
		{
			return Enum.GetValues<GenderType>()
				.Select(e => new Gender { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<Locale> Locales()
		{
			return Enum.GetValues<LocaleType>()
				.Select(e => new Locale { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<Priority> Priorities()
		{
			return Enum.GetValues<PriorityType>()
				.Select(e => new Priority { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<Relationship> Relationships()
		{
			return Enum.GetValues<RelationshipType>()
				.Select(e => new Relationship { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<SocialStatus> SocialStatus()
		{
			return Enum.GetValues<SocialStatusType>()
				.Select(e => new SocialStatus { Id = (byte)e, Name = e.ToString() });
		}

		public static IEnumerable<Status> Status()
		{
			return Enum.GetValues<StatusType>()
				.Select(e => new Status { Id = (byte)e, Name = e.ToString() });
		}
	}
}