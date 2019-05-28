using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnitSense.Extensions.Helpers;

namespace UnitSense.Extensions.Extensions
{
    public static class DateTimeExt
    {
        /// <summary>
        /// Transforme une date en timestamp UNIX
        /// </summary>
        /// <param name="value">Date concerncée</param>
        /// <param name="convertUtc1">Converti explicitement une date UTC 1 en DateTime Local</param>
        /// <returns></returns>
        public static double ToUnixTimestamp(this DateTime value, bool convertUtc1 = false)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            value = convertUtc1 ? value.FromUTC1ToLocal() : value;
            var epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (value - epochDate);

            //return the total seconds (which is a UNIX timestamp)
            return span.TotalSeconds;
        }

        public static int ToUnixTimestampSec(this DateTime date)
        {
            return (int) date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }


        /// <summary>
        /// Donne le nombre d'années qui séparent deux dates
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public static double TotalYears(this DateTime startValue, DateTime endValue)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);


            TimeSpan span = endValue - startValue;
            double
                years = (zeroTime + span).Year -
                        1; // because we start at year 1 for the Gregorian calendar, we must subtract a year here.

            return years; // 1, where my other algorithm resulted in 0.
        }

        /// <summary>
        /// Obtient la date du début de la semaine de la date donnée
        /// </summary>
        /// <param name="dt">Date concernée par la recherche du début de semaine</param>
        /// <param name="startOfWeek">Spécifie le jour de la semaine considéré comme le premier de celle-ci</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Obtient le numéro de la semaine de la date donnée
        /// </summary>
        /// <param name="dtPassed">Date concernée </param>
        /// <returns></returns>
        public static int GetWeekNumber(this DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }


        /// <summary>
        /// Créer une date à partir d'un timestamp UNIX
        /// </summary>
        /// <param name="unixTs"></param>
        /// <returns></returns>
        public static DateTime ToDate(this long unixTs)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTs).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Renvoie le 1er jour du mois courant
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        public static string ToRfc822(this DateTime date, bool isUtc)
        {
            int offset = date.Kind == DateTimeKind.Utc || isUtc ? 0 : TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;

            string timeZone = "+" + offset.ToString().PadLeft(2, '0');


            if (offset < 0)

            {
                int i = offset * -1;

                timeZone = "-" + i.ToString().PadLeft(2, '0');
            }


            return date.ToString($"ddd, dd MMM yyyy HH:mm:ss {timeZone.PadRight(5, '0')}",
                CultureInfo.InvariantCulture);
        }
    }
}