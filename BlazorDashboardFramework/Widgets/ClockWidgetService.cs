using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Widgets
{
    public class ClockWidgetService : IAsyncDisposable
    {

        private readonly Task timerTask;
        private readonly PeriodicTimer timer;
        private readonly CancellationTokenSource cts = new();
        public DateTime CurrentDateTime = DateTime.Now;
        public event EventHandler<DateTime>? CurrentDateTimeChanged;
        public ClockWidgetService()
        {
            timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1000));
            timerTask = DoWorkAsync();
        }

        private async Task DoWorkAsync()
        {
            try
            {
                while (await timer.WaitForNextTickAsync(cts.Token))
                {
                    CurrentDateTime = DateTime.Now;
                    CurrentDateTimeChanged?.Invoke(this, CurrentDateTime);
                }
            }
            catch (OperationCanceledException) { }
        }

        public async ValueTask DisposeAsync()
        {
            if (timerTask is null)
                return;
            cts.Cancel();
            await timerTask;
            cts.Dispose();
        }
    }
}
