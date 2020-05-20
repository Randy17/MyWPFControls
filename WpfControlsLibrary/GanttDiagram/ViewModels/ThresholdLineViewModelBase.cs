using System.Windows.Media;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    internal abstract class ThresholdLineViewModelBase<TThresholdLine> : ViewModelBase 
        where TThresholdLine : IThresholdLine
    {
        protected readonly TThresholdLine _thresholdLine;

        private string _description;
        private Brush _brush;
        private int _position;

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        public Brush Brush
        {
            get => _brush;
            set
            {
                if (_brush != value)
                {
                    _brush = value;
                    RaisePropertyChanged(nameof(Brush));
                }
            }
        }

        public int Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    RaisePropertyChanged(nameof(Position));
                }
            }
        }

        public TThresholdLine ThresholdLine => _thresholdLine;

        protected ThresholdLineViewModelBase(TThresholdLine thresholdLine)
        {
            _thresholdLine = thresholdLine;

            Description = thresholdLine.Description;
            Brush = thresholdLine.Brush;
            Position = 0;
        }
    }
}
