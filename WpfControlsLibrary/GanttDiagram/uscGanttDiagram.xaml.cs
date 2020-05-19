using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.GanttDiagram.ViewModels;
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
                            diagramViewModel.IsRangeSelectorVisible = ganttDiagram.IsRangeSelectorVisible;

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

                        if (args.OldValue is INotifyCollectionChanged oldObservableCollection)
                        {
                            oldObservableCollection.CollectionChanged -= ganttDiagram.ItemsSource_CollectionChanged;
                        }

                        if (args.NewValue is INotifyCollectionChanged newObservableCollection)
                        {
                            newObservableCollection.CollectionChanged += ganttDiagram.ItemsSource_CollectionChanged;
                        }
                    }
                }));

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems.OfType<IGanttItem>())
                {
                    GanttBehavior.AddItem(newItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in e.OldItems.OfType<IGanttItem>())
                {
                    GanttBehavior.RemoveItem(oldItem);
                }
            }
        }

        #endregion

        #region SelectedItem

        public IGanttItem SelectedItem
        {
            get { return (IGanttItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(IGanttItem), typeof(uscGanttDiagram), new PropertyMetadata(null));

        #endregion

        #region IsRangeSelectorVisible

        public bool IsRangeSelectorVisible
        {
            get { return (bool)GetValue(IsRangeSelectorVisibleProperty); }
            set { SetValue(IsRangeSelectorVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRangeSelectorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRangeSelectorVisibleProperty =
            DependencyProperty.Register("IsRangeSelectorVisible", typeof(bool), typeof(uscGanttDiagram), new PropertyMetadata(false,
                (o, args) =>
                {
                    if (o is uscGanttDiagram ganttDiagram)
                    {
                        if (ganttDiagram.GanttBehavior != null)
                            ganttDiagram.GanttBehavior.IsRangeSelectorVisible = (bool) args.NewValue;
                    }
                } ));

        #endregion

        #region IsRowsShrinked

        public bool IsRowsShrinked
        {
            get { return (bool)GetValue(IsRowsShrinkedProperty); }
            set { SetValue(IsRowsShrinkedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRowsShrinked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRowsShrinkedProperty =
            DependencyProperty.Register("IsRowsShrinked", typeof(bool), typeof(uscGanttDiagram), new PropertyMetadata(false,
                (o, args) =>
                {
                    if (o is uscGanttDiagram ganttDiagram)
                    {
                        if (args.NewValue is bool isShrinked)
                        {
                            if(isShrinked)
                                ganttDiagram.GanttBehavior.ShrinkAllRowsCmd.Execute(null);
                            else
                                ganttDiagram.GanttBehavior.UnshrinkAllRowsCmd.Execute(null);
                        }
                    }
                }));

        #endregion

        #region RowHeaderTemplate

        public DataTemplate RowHeaderTemplate
        {
            get { return (DataTemplate)GetValue(RowHeaderTemplateProperty); }
            set { SetValue(RowHeaderTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeaderTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeaderTemplateProperty =
            DependencyProperty.Register("RowHeaderTemplate", typeof(DataTemplate), typeof(uscGanttDiagram), new PropertyMetadata(null));

        #endregion

        #region GantItemBorderStyle

        public Style GanttItemBorderStyle
        {
            get { return (Style)GetValue(GanttItemBorderStyleProperty); }
            set { SetValue(GanttItemBorderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GanttItemBorderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GanttItemBorderStyleProperty =
            DependencyProperty.Register("GanttItemBorderStyle", typeof(Style), typeof(uscGanttDiagram), new PropertyMetadata(null));

        #endregion

        #region ThresholdLines

        public IList ThresholdLines
        {
            get { return (IList)GetValue(ThresholdLinesProperty); }
            set { SetValue(ThresholdLinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThresholdLines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThresholdLinesProperty =
            DependencyProperty.Register("ThresholdLines", typeof(IList), typeof(uscGanttDiagram), new PropertyMetadata(new List<ThresholdLineBase>(), (o, args) =>
            {
                if (o is uscGanttDiagram ganttDiagram)
                {
                    ganttDiagram.UpdateDiagrammViewModel();

                    if (args.OldValue is INotifyCollectionChanged oldObservableCollection)
                    {
                        oldObservableCollection.CollectionChanged -= ganttDiagram.ThresholdLines_CollectionChanged;
                    }

                    if (args.NewValue is INotifyCollectionChanged newObservableCollection)
                    {
                        newObservableCollection.CollectionChanged += ganttDiagram.ThresholdLines_CollectionChanged;
                    }
                }
            }));

        private void ThresholdLines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems.OfType<ThresholdLineBase>())
                {
                    GanttBehavior.AddThresholdLine(newItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in e.OldItems.OfType<ThresholdLineBase>())
                {
                    GanttBehavior.RemoveThresholdLine(oldItem);
                }
            }
        }

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
            if (GanttBehavior != null)
            {
                if (ItemsSource != null)
                {
                    if (!GanttBehavior.TrySetItems(ItemsSource))
                    {
                        Debug.WriteLine(
                            $"uscGanttDiagramm: Cannot set items of type {ItemsSource.GetType()} to ItemsSource property");
                    }
                }

                if (ThresholdLines != null)
                {
                    if (!GanttBehavior.TrySetThresholdLines(ThresholdLines))
                    {
                        Debug.WriteLine(
                            $"uscGanttDiagramm: Cannot set items of type {ThresholdLines.GetType()} to ThresholdLines property");
                    }
                }
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
            // ReSharper disable once CompareOfFloatsByEqualityOperator
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

        private void ScaleScroll_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0)
            {
                rightScroll.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
        }

        #endregion
    }
}
