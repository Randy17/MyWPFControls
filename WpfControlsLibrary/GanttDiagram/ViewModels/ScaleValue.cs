using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    public class ScaleValue : ViewModelBase
    {
        int _margin;
        string _value;

        public int Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
            }
        }
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
    }
}
