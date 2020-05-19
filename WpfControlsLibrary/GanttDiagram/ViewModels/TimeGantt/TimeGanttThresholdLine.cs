using System;
using System.Windows.Media;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    public class TimeGanttThresholdLine : ThresholdLineBase
    {
        private DateTime _timePosition;

        public DateTime TimePosition
        {
            get => _timePosition;
            set
            {
                if (_timePosition != value)
                {
                    _timePosition = value;
                    RaisePropertyChanged(nameof(TimePosition));
                }
            }
        }
    }
}
