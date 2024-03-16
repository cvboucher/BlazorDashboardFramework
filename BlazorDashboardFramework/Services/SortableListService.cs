using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class SortableListService
    {
        public EventHandler<SortableListEventArgs>? ItemRemoved;
        public EventHandler<SortableListEventArgs>? ItemAdded;

        public void OnItemRemoved(SortableListEventArgs e)
        {
            ItemRemoved?.Invoke(this, e);
        }

        public void OnItemAdded(SortableListEventArgs e)
        {
            ItemAdded?.Invoke(this, e);
        }
    }
}
