using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    internal class TimeGanttThresholdLineViewModel : ThresholdLineViewModelBase<TimeGanttThresholdLine>
    {
        private readonly TimeGanttDiagramViewModel _ganttDiagram;

        public TimeGanttThresholdLineViewModel(TimeGanttThresholdLine thresholdLine, TimeGanttDiagramViewModel ganttDiagram) 
            : base(thresholdLine)
        {
            _ganttDiagram = ganttDiagram;
            RecalculateThresholdLinePosition(thresholdLine);

            _ganttDiagram.ScaleStepChanged += (sender, args) => RecalculateThresholdLinePosition(_thresholdLine);
            _thresholdLine.TimePositionChanged += () => RecalculateThresholdLinePosition(_thresholdLine);
        }

        private void RecalculateThresholdLinePosition(TimeGanttThresholdLine thresholdLine)
        {
            Position = (int)((thresholdLine.TimePosition - _ganttDiagram.StartTime).Ticks / _ganttDiagram.ScaleResolution);
        }
    }
}
