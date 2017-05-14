using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniERP_desktop.Helpers
{
    public static class Utils
    {
        public static string ToCurrency(this decimal number)
        {
            return ToCurrency(number, string.Empty);
        }

        public static string ToCurrency(this decimal number, string symbol)
        {
            return string.Format("{1:N2} {0}", symbol, number);
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }
    }

    public static class DateTimeExtensions
    {
        public static string GetShortYear(this DateTime date)
        {
            return date.Year.ToString().Substring(2);
        }

        public static int GetWeekNumber(this DateTime date)
        {
            return GetWeekNumber(date, CultureInfo.CurrentCulture.Name);
        }

        public static int GetWeekNumber(this DateTime date, string culture)
        {
            CultureInfo info = new CultureInfo(culture);
            return info.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}

