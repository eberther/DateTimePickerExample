using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace DateTimePickerExample.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this IEnumerable<object> value)
        {
            var Months = new Dictionary<string, string>();
            var val = (value as IList);
            if (val == null)
            {
                return DateTime.MinValue;
            }
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3), CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(i));
            }

            int year = int.Parse(val[0].ToString());
            int month = DateTime.ParseExact(Months[val[1].ToString()], "MMMM", CultureInfo.InvariantCulture).Month;
            int day = int.Parse(val[2].ToString());
            int hour = int.Parse(val[3].ToString());
            int minute = int.Parse(val[4].ToString());
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}
