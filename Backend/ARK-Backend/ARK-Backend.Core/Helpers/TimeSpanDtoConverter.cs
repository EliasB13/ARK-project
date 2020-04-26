using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Helpers
{
	public static class TimeSpanDtoConverter
	{
		public static string TimeSpanToString(this TimeSpan time)
		{
			return time.ToString("hh\\:mm");
		}

		public static TimeSpan StringToTimeSpan(this string time)
		{
			return TimeSpan.Parse(time);
		}
	}
}
