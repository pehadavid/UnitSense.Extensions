using System;

namespace UnitSense.Extensions.Extensions
{
    public static class TimeSpanHelper
    {
        public static TimeSpan FromMonthes(int monthes) 
        {
            return DateTime.UtcNow.AddMonths(1) - DateTime.UtcNow;
        }

        public static TimeSpan FromYears(int i)
        {
            return DateTime.UtcNow.AddYears(1) - DateTime.UtcNow;
        }
        
        public static int GetMonths(this TimeSpan timespan)
        {
            return (int)(timespan.Days/30.436875);
        }

        public static int UnixTimestamp()
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}