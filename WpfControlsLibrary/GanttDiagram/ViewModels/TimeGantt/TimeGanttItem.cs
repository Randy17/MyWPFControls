using System;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    public class TimeGanttItem
    {
        public DateTime StartTime
        {
            get;
            set;
        }
        public DateTime EndTime
        {
            get;
            set;
        }
        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }
        public string RowName
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

        public TimeGanttItem()
        {
        }

        public TimeGanttItem(DateTime startTime, DateTime endTime, string rowName, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            RowName = rowName;
            Name = name;
        }
    }
}
