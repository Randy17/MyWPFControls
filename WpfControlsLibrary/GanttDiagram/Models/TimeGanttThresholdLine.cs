using System;
using System.Windows.Media;

namespace WpfControlsLibrary.GanttDiagram.Models
{
    public class TimeGanttThresholdLine : IThresholdLine
    {
        private DateTime _timePosition;

        public string Description { get; set; }
        public Brush Brush { get; set; }
        public DateTime TimePosition
        {
            get => _timePosition;
            set
            {
                if (_timePosition != value)
                {
                    _timePosition = value;
                    TimePositionChanged();
                }
            }
        }

        public event Action TimePositionChanged = delegate { };
    }
}
