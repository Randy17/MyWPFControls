using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    public abstract class ThresholdLineBase : ViewModelBase
    {
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
    }
}
