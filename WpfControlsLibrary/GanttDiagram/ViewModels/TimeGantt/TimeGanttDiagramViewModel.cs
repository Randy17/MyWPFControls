using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;
using WpfControlsLibrary.GanttDiagram.Models;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    internal class TimeGanttDiagramViewModel : GanttDiagramViewModelBase<TimeGanttItem>
    {
        #region Fields
        private long _minScaleStepTicks;
        private long _maxScaleStepTicks;
        private long _scaleStepTicks;
        private long _roundTicks = TimeSpan.FromMinutes(10).Ticks;
        private DateTime _startRangeTime;
        private DateTime _endRangeTime;
        private ObservableCollection<TimeGanttItem> _items;
        private TimeGanttItem _selectedItem;
        private GanttItemViewModelBase _selectedGanttItemViewModelBase;
        #endregion

        #region Properties
        public DateTime StartTime
        {
            get;
            set;
        }
        public DateTime EndTime
        {
            get;
            set;
        }
        public TimeSpan ScaleTimeSpan
        {
            get;
            private set;
        }
        public double ScaleResolution
        {
            get;
            private set;
        }
        public override int ScaleStep
        {
            get
            {
                return base.ScaleStep;
            }
            set
            {
                if (ScaleTimeSpan.TotalHours > 6)
                {
                    ScaleResolution = _scaleStepTicks / (double)value;
                }
                else
                {
                    ScaleResolution = _scaleStepTicks / (double)value;
                }
                LeftRangeSelectorPosition = LeftRangeSelectorPosition * value / ScaleStep;
                RangeWidth = RangeWidth * value / ScaleStep;
                base.ScaleStep = value;
            }
        }
        public override double LeftRangeSelectorPosition
        {
            get
            {
                return base.LeftRangeSelectorPosition;
            }
            set
            {
                base.LeftRangeSelectorPosition = value;
                StartRangeTime = StartTime.AddTicks((long)(value * ScaleResolution));
            }
        }
        public override double RangeWidth
        {
            get
            {
                return base.RangeWidth;
            }
            set
            {
                base.RangeWidth = value;
                EndRangeTime = StartTime.AddTicks((long)((LeftRangeSelectorPosition + value) * ScaleResolution));
            }
        }
        public DateTime StartRangeTime
        {
            get
            {
                return _startRangeTime;
            }
            set
            {
                _startRangeTime = value;
                RaisePropertyChanged("StartRangeTime");
            }
        }
        public DateTime EndRangeTime
        {
            get
            {
                return _endRangeTime;
            }
            set
            {
                _endRangeTime = value;
                RaisePropertyChanged("EndRangeTime");
            }
        }

        public override ObservableCollection<TimeGanttItem> Items
        {
            get { return _items; }
            protected set
            {
                if(_items == value)
                    return;

                Rows.Clear();

                _items = value;
                RaisePropertyChanged(nameof(Items));
                if (_items != null && _items.Count > 0)
                {
                    IsVisible = true;
                    StartTime = _items.Min(i => i.StartTime);
                    StartTime = new DateTime(StartTime.Ticks - (StartTime.Ticks % _roundTicks));
                    EndTime = _items.Max(i => i.EndTime);
                    EndTime = new DateTime(EndTime.Ticks + _roundTicks - (EndTime.Ticks % _roundTicks));
                    TimeSpan minDuration = _items.Where(i => i.Duration > TimeSpan.FromSeconds(0)). Min(i => i.Duration);
                    ScaleTimeSpan = EndTime - StartTime;
                    _scaleStepTicks = TimeSpan.FromMinutes(Math.Ceiling((ScaleTimeSpan.TotalMinutes / 5f / 10)) * 10).Ticks;

                    _minScaleStepTicks = TimeSpan.FromMinutes(10).Ticks;
                    _maxScaleStepTicks = TimeSpan.FromHours(1).Ticks;
                    ScaleResolution = _scaleStepTicks / (double)ScaleStep;

                    StartRangeTime = StartTime.AddTicks((long)(LeftRangeSelectorPosition * ScaleResolution));
                    EndRangeTime = StartTime.AddTicks((long)((LeftRangeSelectorPosition + RangeWidth) * ScaleResolution));

                    double maxResolution = minDuration.Ticks / 10f;
                    if (maxResolution < ScaleResolution)
                    {
                        MaxScaleStepValue = (int)(_scaleStepTicks / maxResolution);
                    }

                    foreach (var timeGanttItem in _items)
                    {
                        AddItemToGraph(timeGanttItem);
                    }
                    _items.CollectionChanged += Items_CollectionChanged;

                    CalculateScaleValues(UscGanttDiagram.graph.ActualWidth);
                }
                else
                {
                    IsVisible = false;
                }
            }
        }

        public override TimeGanttItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged(nameof(SelectedItem));
                    RaiseSelectedItemChanged(_selectedItem);
                }
            }
        }

        #endregion
        

        public TimeGanttDiagramViewModel()
        {
            LeftRangeSelectorPosition = 0;
            IsRangeSelectorVisible = true;
            MaxScaleStepValue = 400;
            IsVisible = false;
            Rows.CollectionChanged += Rows_CollectionChanged;
        }

        private void AddItemToGraph(TimeGanttItem item)
        {
            bool needRecalculateScale = false;
            if (item.StartTime < StartTime)
            {
                StartTime = new DateTime(item.StartTime.Ticks - (item.StartTime.Ticks % _roundTicks));

                foreach (var ganttItem in Rows.SelectMany(r => r.Items))
                {
                    ganttItem.RecalculatePosition();
                }

                needRecalculateScale = true;
            }

            if (item.EndTime > EndTime)
            {
                EndTime = new DateTime(item.EndTime.Ticks + _roundTicks - (item.EndTime.Ticks % _roundTicks));
                needRecalculateScale = true;
            }

            if (needRecalculateScale)
            {
                CalculateScaleValues(UscGanttDiagram.graph.ActualWidth);
            }

            GanttRowViewModelBase row = Rows.FirstOrDefault(r => r.Caption == item.RowName);

            if (row == null)
            {
                row = new GanttRowViewModelBase(item.RowName, this) {Position = Rows.Count + 1};
                Rows.Add(row);
            }

            GanttItemViewModelBase rowItem = new TimeGanttItemViewModel(row, item);
            rowItem.IsSelectedChanged += RowItemOnIsSelectedChanged;
            row.Items.Add(rowItem);
        }

        public override void CalculateScaleValues(double graphWidth)
        {
            ScaleValues.Clear();
            int border = ((int)graphWidth / ScaleStep) + 1;
            //long ts = TimeSpan.FromHours(1).Ticks;
            for (int i = 0; i < border; i++)
            {
                ScaleValue sv = new ScaleValue();
                DateTime newDt = StartTime.AddTicks((long)(ScaleResolution * i * ScaleStep));
                sv.Value = string.Format("{0:HH:mm:ss}", newDt);
                FormattedText formattedText = new FormattedText(sv.Value,
                                                                System.Globalization.CultureInfo.CurrentCulture,
                                                                System.Windows.FlowDirection.LeftToRight,
                                                                new Typeface("Sergoe UI"),
                                                                12, Brushes.Black);
                if (i == 0)
                    sv.Margin = 0;
                else
                    sv.Margin = ScaleStep * i - ((int)formattedText.Width / 2);
                ScaleValues.Add(sv);
            }
        }

        public override bool TrySetItems(IList newItems)
        {
            var type = newItems.GetType();
            if (typeof(TimeGanttItem).IsAssignableFrom(type.GenericTypeArguments[0]))
            {
                Items = new ObservableCollection<TimeGanttItem>(newItems.Cast<TimeGanttItem>());
                return true;
            }

            return false;
        }

        public override void AddItem(IGanttItem newItem)
        {
            if (newItem is TimeGanttItem timeGanttItem)
            {
                Items.Add(timeGanttItem);
            }
        }

        public override void RemoveItem(IGanttItem item)
        {
            if (item is TimeGanttItem timeGanttItem)
            {
                Items.Remove(timeGanttItem);
            }
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    if(newItem is TimeGanttItem timeGanttItem)
                        AddItemToGraph(timeGanttItem);
                }
            }
        }

        private void RowItemOnIsSelectedChanged(GanttItemViewModelBase sender)
        {
            if (sender is TimeGanttItemViewModel timeGanttItem)
            {
                if (_selectedGanttItemViewModelBase != timeGanttItem)
                {
                    if(_selectedGanttItemViewModelBase != null)
                        _selectedGanttItemViewModelBase.IsSelected = false;
                    SelectedItem = timeGanttItem.Content as TimeGanttItem;
                    _selectedGanttItemViewModelBase = timeGanttItem;
                }
            }
        }
    }
}
