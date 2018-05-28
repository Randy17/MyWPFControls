using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlsLibrary.GanttDiagram.ViewModels;

namespace WpfControlsLibrary.GanttDiagram
{
    /// <summary>
    /// Логика взаимодействия для uscGanttDiagram.xaml
    /// </summary>
    public partial class uscGanttDiagram : UserControl
    {
        #region Fields
        private bool _canScroll;
        #endregion

        #region Dependency Properties
        #region ScaleStep
        public int ScaleStep
        {
            get { return (int)GetValue(ScaleStepProperty); }
            set { SetValue(ScaleStepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleStepProperty =
            DependencyProperty.Register("ScaleStep", typeof(int), typeof(uscGanttDiagram), new PropertyMetadata(200));

        #endregion

        #region CanEdit
        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(uscGanttDiagram), new PropertyMetadata(false));
        #endregion

        #region DiagramBackground
        public Brush DiagramBackground
        {
            get { return (Brush)GetValue(DiagramBackgroundProperty); }
            set { SetValue(DiagramBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DiagramBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiagramBackgroundProperty =
            DependencyProperty.Register("DiagramBackground", typeof(Brush), typeof(uscGanttDiagram),
                new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE4ECF7"))));
        #endregion

        #region RowsHeadersBackground
        public Brush RowsHeadersBackground
        {
            get { return (Brush)GetValue(RowsHeadersBackgroundProperty); }
            set { SetValue(RowsHeadersBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowsHeadersBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsHeadersBackgroundProperty =
            DependencyProperty.Register("RowsHeadersBackground", typeof(Brush), typeof(uscGanttDiagram),
                new PropertyMetadata(new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFC0E4FD"))));
        #endregion
        #endregion


        public uscGanttDiagram()
        {
            InitializeComponent();
        }

        private void leftScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void UscGanttDiagram_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is GanttDiagramViewModelBase diagramViewModel)
            {
                this.Visibility = Visibility.Visible;
                //(DataContext as GraphBaseViewModel).ScaleStep = ScaleStep;
                ScaleStep = diagramViewModel.ScaleStep;
                diagramViewModel.uscGanttDiagram = this;
                diagramViewModel.CalculateScaleValues(graph.ActualWidth);
            }
            else
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        private void MainGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                int delta = (int)((scaleSlider.Maximum - scaleSlider.Minimum) * 0.1);
                if (e.Delta > 0)
                {
                    double newValue = scaleSlider.Value + delta;
                    if (newValue <= scaleSlider.Maximum)
                        scaleSlider.Value = newValue;
                    else
                        scaleSlider.Value = scaleSlider.Maximum;
                }
                else if (e.Delta < 0)
                {
                    double newValue = scaleSlider.Value - delta;
                    if (newValue >= scaleSlider.Minimum)
                        scaleSlider.Value = newValue;
                    else
                        scaleSlider.Value = scaleSlider.Minimum;
                }
            }
        }

        private void RightScroll_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0)
            {
                leftScroll.ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void RightScroll_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _canScroll = true;
            }
            else
            {
                _canScroll = false;
                e.Handled = true;
            }
        }

        private void Graph_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is GanttDiagramViewModelBase ganttDiagramViewModel)
                ganttDiagramViewModel.CalculateScaleValues(graph.ActualWidth);
        }
    }
}
