using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class EditModeService
    {

        private bool editMode;
        public bool EditMode
        {
            get => editMode;
            set
            {
                if (editMode != value)
                {
                    editMode = value;
                    EditModeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler? EditModeChanged;
    }
}
