namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    public sealed class TimeGanttItemViewModel : GanttItemViewModelBase
    {
        private readonly TimeGanttItem _contentItem;

        public override int ScaleStep
        {
            get
            {
                return base.ScaleStep;
            }
            set
            {
                if (_scaleStep != value)
                {
                    _scaleStep = value;
                    _startPosition = (int)((_contentItem.StartTime - ((TimeGanttDiagramViewModel)GanttRow.GanttDiagram).StartTime).Ticks / ((TimeGanttDiagramViewModel)GanttRow.GanttDiagram).ScaleResolution);
                    _duration = (int)((_contentItem.EndTime - _contentItem.StartTime).Ticks / ((TimeGanttDiagramViewModel) GanttRow.GanttDiagram).ScaleResolution);
                    RaisePropertyChanged(nameof(StartPosition));
                    RaisePropertyChanged(nameof(Duration));
                }
            }
        }

        public TimeGanttItemViewModel(GanttRowViewModelBase parentRow, TimeGanttItem item) : base(parentRow)
        {
            _contentItem = item;
            Caption = item.Name;
            Content = item;

            StartPosition = (int)((item.StartTime - ((TimeGanttDiagramViewModel) parentRow.GanttDiagram).StartTime).Ticks / ((TimeGanttDiagramViewModel) parentRow.GanttDiagram).ScaleResolution);
            Duration = (int)((item.EndTime - item.StartTime).Ticks / ((TimeGanttDiagramViewModel) parentRow.GanttDiagram).ScaleResolution);
        }
    }
}
