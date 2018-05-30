﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.GanttDiagram.ViewModels.Interfaces;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    internal class GanttDiagramViewModelBase <TGanttItem> : ViewModelBase, IGanttDiagramViewModel
        where TGanttItem : IGanttItem
    {
        #region Fields

        private int _scaleStep = 200;
        protected bool _isVisible;
        private string _serviceMessage;
        private int _maxWidth = 20075;
        private ObservableCollection<ScaleValue> _scaleValues;
        private ObservableCollection<GanttRowViewModelBase> _rows;
        private bool _isRangeSelectorVisible;
        private double _leftRangeSelectorPosition;
        private double _rangeWidth;

        #endregion

        #region Properties

        public virtual bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                if (_isVisible == false)
                {
                    ServiceMessage = "Отсутствуют данные для построения графика";
                }
                RaisePropertyChanged(nameof(IsVisible));
            }
        }
        public string ServiceMessage
        {
            get
            {
                return _serviceMessage;
            }
            set
            {
                _serviceMessage = value;
                RaisePropertyChanged(nameof(ServiceMessage));
            }
        }
        public uscGanttDiagram UscGanttDiagram { get; set; }
        public virtual int ScaleStep
        {
            get
            {
                return _scaleStep;
            }
            set
            {
                _scaleStep = value;
                MaxWidth = _scaleStep * 100;
                RaisePropertyChanged(nameof(ScaleStep));
                ScaleStepChanged(this, new EventArgs());
                if (UscGanttDiagram != null)
                {
                    CalculateScaleValues(UscGanttDiagram.graph.ActualWidth);
                }
            }
        }
        public int MaxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value + 75;
                RaisePropertyChanged(nameof(MaxWidth));
            }
        }
        public ObservableCollection<ScaleValue> ScaleValues
        {
            get
            {
                return _scaleValues;
            }
            set
            {
                _scaleValues = value;
                RaisePropertyChanged(nameof(ScaleValues));
            }
        }
        public ObservableCollection<GanttRowViewModelBase> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
                RaisePropertyChanged(nameof(Rows));
            }
        }
        public virtual string Caption
        {
            get
            {
                return null;
            }
        }
        public int MinScaleStepValue
        {
            get;
            set;
        }
        public int MaxScaleStepValue
        {
            get;
            set;
        }
        public bool IsRangeSelectorVisible
        {
            get
            {
                return _isRangeSelectorVisible;
            }
            set
            {
                _isRangeSelectorVisible = value;
                RaisePropertyChanged(nameof(IsRangeSelectorVisible));
            }
        }
        public virtual double LeftRangeSelectorPosition
        {
            get
            {
                return _leftRangeSelectorPosition;
            }
            set
            {
                _leftRangeSelectorPosition = value;
                RaisePropertyChanged(nameof(LeftRangeSelectorPosition));
            }
        }
        public virtual double RangeWidth
        {
            get
            {
                return _rangeWidth;
            }
            set
            {
                _rangeWidth = value;
                RaisePropertyChanged(nameof(RangeWidth));
            }
        }
        public virtual ObservableCollection<TGanttItem> Items { get; protected set; }
        ObservableCollection<IGanttItem> IGanttDiagramViewModel.Items => Items as ObservableCollection<IGanttItem>;
        public virtual TGanttItem SelectedItem { get; set; }
        IGanttItem IGanttDiagramViewModel.SelectedItem => SelectedItem;

        #endregion

        #region Events

        public event EventHandler ScaleStepChanged = delegate { };
        public event EventHandler GraphUpdated = delegate { };
        public event SelectedItemChangedDelegate SelectedItemChanged = delegate { };

        #endregion

        public GanttDiagramViewModelBase()
        {
            Rows = new ObservableCollection<GanttRowViewModelBase>();
            ScaleValues = new ObservableCollection<ScaleValue>();
            MinScaleStepValue = 70;
            MaxScaleStepValue = 280;
        }

        #region Methods

        protected virtual void Rows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as GanttRowViewModelBase).Position = Rows.Count;
                GanttRowViewModelBase row = (e.NewItems[0] as GanttRowViewModelBase);
                IsVisible = true;
                GraphUpdated(this, new EventArgs());
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < Rows.Count; i++)
                {
                    Rows[i].Position = i + 1;
                }

                GraphUpdated(this, new EventArgs());
            }
        }
        public virtual void CalculateScaleValues(double graphWidth)
        {
            ScaleValues.Clear();
            //ScaleValues.Add(new ScaleValue() { Value = 0, Margin = 0 });
            int border = (int)(graphWidth / ScaleStep);
            for (int i = 0; i < border; i++)
            {
                ScaleValue sv = new ScaleValue();
                sv.Value = string.Format("Этап {0}", i + 1);
                FormattedText formattedText = new FormattedText(sv.Value.ToString(),
                    System.Globalization.CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Sergoe UI"),
                    12, System.Windows.Media.Brushes.Black);
                double textWidth = formattedText.Width;
                //sv.Margin = ScaleStep * (i + 1) - (int)formattedText.Width / 2;
                sv.Margin = ScaleStep * i + (ScaleStep / 2 - (int)formattedText.Width / 2);
                ScaleValues.Add(sv);
            }
        }
        public virtual bool TrySetItems(object newItems)
        {
            return true;
        }

        protected void RaiseSelectedItemChanged(TGanttItem newSelectedItem)
        {
            SelectedItemChanged(newSelectedItem);
        }

        #endregion
    }
}
