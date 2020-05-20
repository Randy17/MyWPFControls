using System.Windows.Media;

namespace WpfControlsLibrary.GanttDiagram.Models
{
    public interface IThresholdLine
    {
        string Description { get; set; }

        Brush Brush { get; set; }
    }
}
