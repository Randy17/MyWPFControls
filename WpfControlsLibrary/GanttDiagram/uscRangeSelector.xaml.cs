using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfControlsLibrary.GanttDiagram
{
    /// <summary>
    /// Логика взаимодействия для uscRangeSelector.xaml
    /// </summary>
    public partial class uscRangeSelector : UserControl
    {
        #region LeftAdornerPosition
        public double LeftAdornerPosition
        {
            get { return (double)GetValue(LeftAdornerPositionProperty); }
            set { SetValue(LeftAdornerPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LeftAdornerPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftAdornerPositionProperty =
            DependencyProperty.Register("LeftAdornerPosition", typeof(double), typeof(uscRangeSelector), new PropertyMetadata(0d, new PropertyChangedCallback(
                (d, e) =>
                {
                    (d as uscRangeSelector)?.OnLeftAdornerPositionChanged((double) e.OldValue, (double) e.NewValue);
                })));

        private void OnLeftAdornerPositionChanged(double oldValue, double newValue)
        {
            if (newValue >= 0 && brdLeftArea.Width != newValue)
            {
                brdLeftArea.Width = newValue;
            }
        }
        #endregion

        #region SelectedAreaWidth
        public double SelectedAreaWidth
        {
            get { return (double)GetValue(SelectedAreaWidthProperty); }
            set { SetValue(SelectedAreaWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedAreaWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedAreaWidthProperty =
            DependencyProperty.Register("SelectedAreaWidth", typeof(double), typeof(uscRangeSelector), new PropertyMetadata(0d, new PropertyChangedCallback(
                (d, e) =>
                {
                    (d as uscRangeSelector).OnSelectedAreaWidthChanged((double) e.OldValue, (double) e.NewValue);
                })));

        private void OnSelectedAreaWidthChanged(double oldValue, double newValue)
        {
            if (newValue >= 0 && brdCentalArea.Width != newValue)
            {
                brdCentalArea.Width = newValue;
            }
        }
        #endregion

        public uscRangeSelector()
        {
            InitializeComponent();
        }

        private void LeftAdorner_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(brdLeftArea.Width == 0 && e.HorizontalChange < 0) && (brdLeftArea.Width + e.HorizontalChange) >= 0)
            {
                brdLeftArea.Width += e.HorizontalChange;
                LeftAdornerPosition = brdLeftArea.Width;

                if (!(brdCentalArea.Width == 0 && e.HorizontalChange > 0) && (brdCentalArea.Width - e.HorizontalChange) >= 0)
                {
                    brdCentalArea.Width -= e.HorizontalChange;
                    SelectedAreaWidth = brdCentalArea.Width;
                }
            }
        }

        private void RightAdorner_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(brdCentalArea.Width == 0 && e.HorizontalChange < 0) && (brdCentalArea.Width + e.HorizontalChange) >= 0)
            {
                brdCentalArea.Width += e.HorizontalChange;
                SelectedAreaWidth = brdCentalArea.Width;
            }
            else if ((brdLeftArea.Width + e.HorizontalChange) >= 0)
            {
                brdLeftArea.Width += e.HorizontalChange;
                LeftAdornerPosition = brdLeftArea.Width;
            }
        }
    }
}
