using GraduationProjectAPI.Enums;

namespace GraduationProjectAPI.Utilities.ExtensionMethods
{
	public static class GeneralExtenders
	{
		public static string ToEnumString(this PeriodType periodType)
		{
			if (periodType == PeriodType.OneTime)
				return "One Time";

			return periodType.ToString();
		}
	}
}
