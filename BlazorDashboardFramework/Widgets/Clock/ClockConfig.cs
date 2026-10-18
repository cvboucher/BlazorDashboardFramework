namespace BlazorDashboardFramework.Widgets.Clock
{
    /// <summary>
    /// Clock settings. Uses the AngularJS clock widget keys (timePattern/datePattern, moment.js patterns)
    /// so existing clocks keep their format.
    /// </summary>
    public class ClockConfig
    {
        public const string DefaultTimePattern = "HH:mm:ss";
        public const string DefaultDatePattern = "YYYY-MM-DD";

        /// <summary>moment.js pattern for the time line, e.g. HH:mm:ss or h:mm A.</summary>
        public string? TimePattern { get; set; } = DefaultTimePattern;

        /// <summary>moment.js pattern for the date line, e.g. YYYY-MM-DD or dddd, MMMM D. Empty hides the date.</summary>
        public string? DatePattern { get; set; } = DefaultDatePattern;

        /// <summary>Legacy .NET format string written by earlier versions of this framework.</summary>
        public string? Format { get; set; }

        public bool HideHeader { get; set; }
    }
}
