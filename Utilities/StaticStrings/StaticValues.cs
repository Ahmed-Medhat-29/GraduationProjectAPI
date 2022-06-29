using System;
using System.Collections.Generic;
using System.Linq;
using GraduationProjectAPI.Enums;
using GraduationProjectAPI.Models.CaseProperties;
using GraduationProjectAPI.Models.Shared;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace GraduationProjectAPI.Utilities.StaticStrings
{
	public static class StaticValues
	{
		public static IEnumerable<Period> Periods()
		{
			return Enum.GetValues<PeriodType>()
				.Select(e => new Period { Id = e, Name = e.ToCustomString() });
		}

		public static IEnumerable<Models.NotificationType> NotificationTypes()
		{
			return Enum.GetValues<NotificationType>()
				.Select(e => new Models.NotificationType { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<Models.MessageType> MessageTypes()
		{
			return Enum.GetValues<MessageType>()
				.Select(e => new Models.MessageType { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<Gender> Genders()
		{
			return Enum.GetValues<GenderType>()
				.Select(e => new Gender { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<Priority> Priorities()
		{
			return Enum.GetValues<PriorityType>()
				.Select(e => new Priority { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<Relationship> Relationships()
		{
			return Enum.GetValues<RelationshipType>()
				.Select(e => new Relationship { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<SocialStatus> SocialStatus()
		{
			return Enum.GetValues<SocialStatusType>()
				.Select(e => new SocialStatus { Id = e, Name = e.ToString() });
		}

		public static IEnumerable<Status> Status()
		{
			return Enum.GetValues<StatusType>()
				.Select(e => new Status { Id = e, Name = e.ToString() });
		}
	}
}