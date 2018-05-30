using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt;

namespace WpfDemoApp.ViewModels
{
    public class ScheduleResultItem : TimeGanttItem
    {
        public string ItemName { get; set; }
        public string OperationName { get; set; }
        public string EquipmentName { get; set; }
        public string PersonnelName { get; set; }
        public double Weight { get; set; }

        public ScheduleResultItem(DateTime startTime, DateTime endTime, string rowName, string name)
            : base(startTime, endTime, rowName, name)
        {

        }
    }
}
