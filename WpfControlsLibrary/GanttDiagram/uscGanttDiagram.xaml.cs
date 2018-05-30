using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.GanttDiagram.ViewModels.Interfaces;
using WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt;

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

        #region SelectedItemBackground

        public Brush SelectedItemBackground
        {
            get { return (Brush)GetValue(SelectedItemBackgroundProperty); }
            set { SetValue(SelectedItemBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemBackgroundProperty =
            DependencyProperty.Register("SelectedItemBackground", typeof(Brush), typeof(uscGanttDiagram), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        #endregion

        #region GanttBehavior

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal IGanttDiagramViewModel GanttBehavior
        {
            get { return (IGanttDiagramViewModel)GetValue(GanttBehaviorProperty); }
            set { SetValue(GanttBehaviorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GanttBehavior.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty GanttBehaviorProperty =
            DependencyProperty.Register("GanttBehavior", typeof(IGanttDiagramViewModel), typeof(uscGanttDiagram), new PropertyMetadata(null,
                (o, args) =>
                {
                    if (o is uscGanttDiagram ganttDiagram)
                    {
                        if (args.OldValue is IGanttDiagramViewModel oldDiagramViewModel)
                        {
                            oldDiagramViewModel.SelectedItemChanged -= ganttDiagram.DiagramViewModelOnSelectedItemChanged;
                        }

                        if (args.NewValue is IGanttDiagramViewModel diagramViewModel)
                        {
                            ganttDiagram.Visibility = Visibility.Visible;
                            ganttDiagram.ScaleStep = diagramViewModel.ScaleStep;
                            diagramViewModel.UscGanttDiagram = ganttDiagram;
                            diagramViewModel.CalculateScaleValues(ganttDiagram.graph.ActualWidth);

                            ganttDiagram.UpdateDiagrammViewModel();
                            diagramViewModel.SelectedItemChanged += ganttDiagram.DiagramViewModelOnSelectedItemChanged;
                        }
                    }
                }));

        #endregion

        #region DiagramType

        public enum DiagramTypeEnum
        {
            TimeGantt
        }

        public DiagramTypeEnum DiagramType
        {
            get { return (DiagramTypeEnum)GetValue(DiagramTypeProperty); }
            set { SetValue(DiagramTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DiagramType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiagramTypeProperty =
            DependencyProperty.Register("DiagramType", typeof(DiagramTypeEnum), typeof(uscGanttDiagram), new PropertyMetadata(DiagramTypeEnum.TimeGantt,
                (o, args) =>
                {
                    if (o is uscGanttDiagram ganttDiagram)
                    {
                        ganttDiagram.DiagramTypeChanged(args.OldValue, args.NewValue);
                    }
                }));

        private void DiagramTypeChanged(object oldValue, object newValue)
        {
            if (newValue is DiagramTypeEnum diagramType)
            {
                if ((DiagramTypeEnum)oldValue != diagramType)
                {
                    switch (diagramType)
                    {
                        case DiagramTypeEnum.TimeGantt:
                            GanttBehavior = new TimeGanttDiagramViewModel();
                            break;
                        default:
                            GanttBehavior = null;
                            break;
                    }

                    UpdateDiagrammViewModel();
                }
            }
        }

        #endregion

        #region ItemsSource

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(uscGanttDiagram), new PropertyMetadata(null,
                (o, args) =>
                {
                    if (o is uscGanttDiagram ganttDiagram)
                    {
                        ganttDiagram.UpdateDiagrammViewModel();
                    }
                }));

        #endregion

        #region SelectedItem

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(uscGanttDiagram), new PropertyMetadata(null));

        #endregion

        #endregion

        public uscGanttDiagram()
        {
            InitializeComponent();

            //TODO: Refactoring
            switch (DiagramType)
            {
                case DiagramTypeEnum.TimeGantt:
                    GanttBehavior = new TimeGanttDiagramViewModel();
                    break;
                default:
                    GanttBehavior = null;
                    break;
            }
        }

        #region Methods

        private void UpdateDiagrammViewModel()
        {
            if (GanttBehavior != null && ItemsSource != null)
            {
                GanttBehavior.TrySetItems(ItemsSource);
            }
        }

        #endregion
        
        #region EventHandlers

        private void leftScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
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
            GanttBehavior?.CalculateScaleValues(graph.ActualWidth);
        }

        private void DiagramViewModelOnSelectedItemChanged(IGanttItem newSelecteditem)
        {
            SelectedItem = newSelecteditem;
        }

        #endregion
    }
}
