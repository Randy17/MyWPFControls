using System;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    public class TimeGanttItem : IGanttItem
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

        public GanttItemInRowPosition InRowPosition { get; set; } = GanttItemInRowPosition.FullRow;

        public TimeGanttItem()
        {
        }

        public TimeGanttItem(DateTime startTime, DateTime endTime, string rowName, string name, GanttItemInRowPosition inRowPostion = GanttItemInRowPosition.FullRow)
        {
            StartTime = startTime;
            EndTime = endTime;
            RowName = rowName;
            Name = name;
            InRowPosition = inRowPostion;
        }
    }
}
