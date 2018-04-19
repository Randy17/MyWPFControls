using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace WpfControlsLibrary.GanttDiagram.ViewModels.TimeGantt
{
    public class TimeGanttDiagramViewModel : GanttDiagramViewModelBase
    {
        #region Fields
        private long _minScaleStepTicks;
        private long _maxScaleStepTicks;
        private long _scaleStepTicks;
        private DateTime _startRangeTime;
        private DateTime _endRangeTime;
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
                    ScaleResolution = _scaleStepTicks / value;
                }
                else
                {
                    ScaleResolution = _scaleStepTicks / value;
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
        #endregion

        public TimeGanttDiagramViewModel(List<TimeGanttItem> items) : base()
        {
            LeftRangeSelectorPosition = 0;
            IsRangeSelectorVisible = true;
            MaxScaleStepValue = 400;
            if (items != null && items.Count > 0)
            {
                IsVisible = true;
                long roundTicks = TimeSpan.FromMinutes(10).Ticks;
                StartTime = items.Min(i => i.StartTime);
                StartTime = new DateTime(StartTime.Ticks - (StartTime.Ticks % roundTicks));
                EndTime = items.Max(i => i.EndTime);
                EndTime = new DateTime(EndTime.Ticks + roundTicks - (EndTime.Ticks % roundTicks));
                TimeSpan minDuration = items.Min(i => i.Duration);
                ScaleTimeSpan = EndTime - StartTime;
                _scaleStepTicks = TimeSpan.FromMinutes(Math.Ceiling((ScaleTimeSpan.TotalMinutes / 5f / 10)) * 10).Ticks;

                _minScaleStepTicks = TimeSpan.FromMinutes(10).Ticks;
                _maxScaleStepTicks = TimeSpan.FromHours(1).Ticks;
                ScaleResolution = _scaleStepTicks / ScaleStep;

                StartRangeTime = StartTime.AddTicks((long)(LeftRangeSelectorPosition * ScaleResolution));
                EndRangeTime = StartTime.AddTicks((long)((LeftRangeSelectorPosition + RangeWidth) * ScaleResolution));

                double maxResolution = minDuration.Ticks / 10f;
                if (maxResolution < ScaleResolution)
                {
                    MaxScaleStepValue = (int)(_scaleStepTicks / maxResolution);
                }
            }
            else
            {
                IsVisible = false;
            }

            
            foreach (var item in items)
            {
                GanttRowViewModelBase row = Rows.FirstOrDefault(r => r.Caption == item.RowName);
                if (row == null)
                {
                    row = new GanttRowViewModelBase(item.RowName, this) { Position = Rows.Count + 1 };
                    row.Items.Add(new TimeGanttItemViewModel(row, item));
                    Rows.Add(row);
                }
                else
                {
                    row.Items.Add(new TimeGanttItemViewModel(row, item));
                }
            }

            Rows.CollectionChanged += Rows_CollectionChanged;
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
                FormattedText formattedText = new FormattedText(sv.Value.ToString(),
                                                                System.Globalization.CultureInfo.CurrentCulture,
                                                                System.Windows.FlowDirection.LeftToRight,
                                                                new Typeface("Sergoe UI"),
                                                                12, System.Windows.Media.Brushes.Black);
                if (i == 0)
                    sv.Margin = 0;
                else
                    sv.Margin = ScaleStep * i - ((int)formattedText.Width / 2);
                ScaleValues.Add(sv);
            }
        }
    }
}
