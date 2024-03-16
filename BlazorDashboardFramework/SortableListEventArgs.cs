using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class SortableListEventArgs : EventArgs
    {
        public SortableListEventArgs(int index, string id, object item)
        {
            Index = index;
            Id = id;
            Item = item;
        }

        public int Index { get; set; }
        public string Id { get; set; }
        public object Item { get; set; }
    }
}
