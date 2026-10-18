using System.Globalization;
using System.Text;

namespace BlazorDashboardFramework.Widgets.Clock
{
    /// <summary>
    /// Formats a date with a moment.js display pattern (https://momentjs.com/docs/#/displaying/format/),
    /// the pattern syntax used by the AngularJS clock widget (timePattern/datePattern).
    /// Text in [brackets] is literal; unknown characters are copied as-is.
    /// </summary>
    public static class MomentFormatter
    {
        // Longest tokens first so e.g. "MMMM" wins over "MM".
        private static readonly string[] Tokens =
        {
            "YYYYYY", "YYYY", "YY", "Y",
            "Qo", "Q",
            "MMMM", "MMM", "Mo", "MM", "M",
            "DDDD", "DDDo", "DDD", "Do", "DD", "D",
            "dddd", "ddd", "do", "dd", "d", "E", "e",
            "wo", "ww", "w", "Wo", "WW", "W",
            "GGGG", "gggg",
            "HH", "H", "hh", "h", "kk", "k",
            "mm", "m", "ss", "s",
            "SSS", "SS", "S",
            "A", "a",
            "ZZ", "Z", "X", "x",
        };

        public static string Format(DateTimeOffset value, string? pattern, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;
            if (string.IsNullOrEmpty(pattern))
                return string.Empty;
            var dt = value.DateTime;
            var sb = new StringBuilder();
            var i = 0;
            while (i < pattern.Length)
            {
                if (pattern[i] == '[')
                {
                    var end = pattern.IndexOf(']', i + 1);
                    if (end > i)
                    {
                        sb.Append(pattern, i + 1, end - i - 1);
                        i = end + 1;
                        continue;
                    }
                }
                var token = Tokens.FirstOrDefault(t => string.CompareOrdinal(pattern, i, t, 0, t.Length) == 0);
                if (token == null)
                {
                    sb.Append(pattern[i]);
                    i++;
                    continue;
                }
                sb.Append(FormatToken(token, value, dt, culture));
                i += token.Length;
            }
            return sb.ToString();
        }

        public static string Format(DateTime value, string? pattern, CultureInfo? culture = null)
            => Format(new DateTimeOffset(value, value.Kind == DateTimeKind.Utc ? TimeSpan.Zero : TimeZoneInfo.Local.GetUtcOffset(value)), pattern, culture);

        private static string FormatToken(string token, DateTimeOffset value, DateTime dt, CultureInfo culture)
        {
            var dtf = culture.DateTimeFormat;
            var dayOfWeek = (int)dt.DayOfWeek;
            var isoDayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
            var localWeek = culture.Calendar.GetWeekOfYear(dt, dtf.CalendarWeekRule, dtf.FirstDayOfWeek);
            var isoWeek = ISOWeek.GetWeekOfYear(dt);
            var quarter = (dt.Month - 1) / 3 + 1;
            var offset = value.Offset;
            var sign = offset < TimeSpan.Zero ? "-" : "+";
            var abs = offset.Duration();
            return token switch
            {
                "YYYYYY" => (dt.Year >= 0 ? "+" : "-") + Math.Abs(dt.Year).ToString("000000", culture),
                "YYYY" => dt.Year.ToString("0000", culture),
                "YY" => (dt.Year % 100).ToString("00", culture),
                "Y" => dt.Year.ToString(culture),
                "Q" => quarter.ToString(culture),
                "Qo" => Ordinal(quarter),
                "MMMM" => dtf.GetMonthName(dt.Month),
                "MMM" => dtf.GetAbbreviatedMonthName(dt.Month),
                "Mo" => Ordinal(dt.Month),
                "MM" => dt.Month.ToString("00", culture),
                "M" => dt.Month.ToString(culture),
                "DDDD" => dt.DayOfYear.ToString("000", culture),
                "DDDo" => Ordinal(dt.DayOfYear),
                "DDD" => dt.DayOfYear.ToString(culture),
                "Do" => Ordinal(dt.Day),
                "DD" => dt.Day.ToString("00", culture),
                "D" => dt.Day.ToString(culture),
                "dddd" => dtf.GetDayName(dt.DayOfWeek),
                "ddd" => dtf.GetAbbreviatedDayName(dt.DayOfWeek),
                "dd" => dtf.GetShortestDayName(dt.DayOfWeek),
                "do" => Ordinal(dayOfWeek),
                "d" => dayOfWeek.ToString(culture),
                "e" => (((dayOfWeek - (int)dtf.FirstDayOfWeek) + 7) % 7).ToString(culture),
                "E" => isoDayOfWeek.ToString(culture),
                "wo" => Ordinal(localWeek),
                "ww" => localWeek.ToString("00", culture),
                "w" => localWeek.ToString(culture),
                "Wo" => Ordinal(isoWeek),
                "WW" => isoWeek.ToString("00", culture),
                "W" => isoWeek.ToString(culture),
                "GGGG" => ISOWeek.GetYear(dt).ToString("0000", culture),
                "gggg" => dt.Year.ToString("0000", culture),
                "HH" => dt.Hour.ToString("00", culture),
                "H" => dt.Hour.ToString(culture),
                "hh" => Hour12(dt).ToString("00", culture),
                "h" => Hour12(dt).ToString(culture),
                "kk" => (dt.Hour == 0 ? 24 : dt.Hour).ToString("00", culture),
                "k" => (dt.Hour == 0 ? 24 : dt.Hour).ToString(culture),
                "mm" => dt.Minute.ToString("00", culture),
                "m" => dt.Minute.ToString(culture),
                "ss" => dt.Second.ToString("00", culture),
                "s" => dt.Second.ToString(culture),
                "SSS" => dt.Millisecond.ToString("000", culture),
                "SS" => (dt.Millisecond / 10).ToString("00", culture),
                "S" => (dt.Millisecond / 100).ToString(culture),
                "A" => AmPm(dt, dtf),
                "a" => AmPm(dt, dtf).ToLower(culture),
                "ZZ" => $"{sign}{abs.Hours:00}{abs.Minutes:00}",
                "Z" => $"{sign}{abs.Hours:00}:{abs.Minutes:00}",
                "X" => value.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
                "x" => value.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture),
                _ => token,
            };
        }

        private static int Hour12(DateTime dt) => dt.Hour % 12 == 0 ? 12 : dt.Hour % 12;

        private static string AmPm(DateTime dt, DateTimeFormatInfo dtf)
        {
            var designator = dt.Hour < 12 ? dtf.AMDesignator : dtf.PMDesignator;
            return string.IsNullOrEmpty(designator) ? (dt.Hour < 12 ? "AM" : "PM") : designator;
        }

        private static string Ordinal(int n)
        {
            var mod100 = n % 100;
            var suffix = mod100 is 11 or 12 or 13 ? "th" : (n % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" };
            return n.ToString(CultureInfo.InvariantCulture) + suffix;
        }
    }
}
