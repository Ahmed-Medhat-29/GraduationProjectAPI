using System;
using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.Utilities
{
	public static class Extenders
	{
		public static string ToCustomString(this PeriodType periodType)
		{
			if (periodType == PeriodType.OneTime)
				return "One Time";

			return periodType.ToString();
		}
	}
}
