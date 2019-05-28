using System;
using System.Runtime.InteropServices;

namespace UnitSense.Extensions.Helpers
{
    public static class DateTimeHelper
    {
        private static Lazy<bool> IsUnix = new Lazy<bool>(() => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux));

        /// <summary>
        /// Obtient la date courante à l'heure de Paris (CET / CEST)
        /// </summary>
        public static DateTime Utc1Now
        {
            get
            {
                TimeZoneInfo timeZoneInfo;
                DateTime dateTime;
                //Set the time zone information to US Mountain Standard Time 
                timeZoneInfo = IsUnix.Value ? TimeZoneInfo.FindSystemTimeZoneById("Europe/Paris") : TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                //Get date and time in US Mountain Standard Time 
                dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                return dateTime;
            }
        }


        /// <summary>
        /// Cette methode Convertit explicitement un DateTime Heure de Paris (CET / CEST) en DateTime Local
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime FromUTC1ToLocal(this DateTime dateTime)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            string tz = timeZoneInfo.IsDaylightSavingTime(dateTime) ? " +02:00" : " +01:00";
            var strDate = dateTime.ToString(InnerFormat);
            DateTime parsed = DateTime.Parse(strDate + tz);
            return parsed.ToLocalTime();
        }


        /// <summary>
        /// Permet d'obtenir le format de date international.
        /// </summary>
        private static string InnerFormat
        {
            get { return "yyyy-MM-dd HH:mm:ss.f"; }
        }


        /// <summary>
        /// Algorithm for calculating the date of Easter Sunday
        /// (Meeus/Jones/Butcher Gregorian algorithm)
        /// http://en.wikipedia.org/wiki/Computus#Meeus.2FJones.2FButcher_Gregorian_algorithm
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime EasterDate(int year)
        {
            int Y = year;
            int a = Y % 19;
            int b = Y / 100;
            int c = Y % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int L = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * L) / 451;
            int month = (h + L - 7 * m + 114) / 31;
            int day = ((h + L - 7 * m + 114) % 31) + 1;
            System.DateTime dt = new System.DateTime(year, month, day);
            return dt;
        }
    }
}