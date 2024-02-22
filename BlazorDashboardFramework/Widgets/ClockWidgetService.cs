using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Widgets
{
    public class ClockWidgetService : IDisposable
    {

        private System.Timers.Timer clockTimer;
        public DateTime CurrentDateTime = DateTime.Now;
        public event EventHandler<DateTime>? CurrentDateTimeChanged;
        public ClockWidgetService()
        {
            clockTimer = new System.Timers.Timer(1000);
            clockTimer.Elapsed += UpdateClock;
            clockTimer.Enabled = true;

        }
        private void UpdateClock(Object? source, System.Timers.ElapsedEventArgs e)
        {
            CurrentDateTime = DateTime.Now;
            CurrentDateTimeChanged?.Invoke(this, CurrentDateTime);
        }

        public void Dispose()
        {
            clockTimer.Elapsed -= UpdateClock;
        }
    }
}
