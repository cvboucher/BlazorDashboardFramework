namespace BlazorDashboardFramework
{
    /// <summary>
    /// Opt-in self-hiding for widgets. <see cref="BdfWidget"/> cascades one instance per widget to its display
    /// component; the widget calls <see cref="SetHidden"/> (for example, when it has no data) and the dashboard
    /// hides the widget's card outside edit mode. Visible by default, so widgets that never use it are unaffected.
    /// </summary>
    /// <example>
    /// <code>
    /// [CascadingParameter] public WidgetVisibility? WidgetVisibility { get; set; }
    /// ...
    /// WidgetVisibility?.SetHidden(!items.Any());
    /// </code>
    /// </example>
    public sealed class WidgetVisibility
    {
        /// <summary>True after the widget called <c>SetHidden(true)</c>.</summary>
        public bool HiddenByWidget { get; private set; }

        /// <summary>Raised when <see cref="HiddenByWidget"/> changes.</summary>
        public event Action? Changed;

        public void SetHidden(bool hidden)
        {
            if (HiddenByWidget == hidden)
                return;
            HiddenByWidget = hidden;
            Changed?.Invoke();
        }

        /// <summary>Whether the card is hidden right now: never in edit mode, so the widget can still be configured or removed.</summary>
        public bool IsHidden(bool editMode) => HiddenByWidget && !editMode;
    }
}
