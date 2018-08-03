using System;
using WpfControlsLibrary.GanttDiagram.Models;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    internal delegate void IsSelectedDelegate(GanttItemViewModelBase sender);

    internal class GanttItemViewModelBase : ViewModelBase
    {
        #region Fields
        private string _caption;
        protected int _startPosition;
        protected int _duration;
        protected bool _isSelected;
        protected int _scaleStep;
        private GanttRowViewModelBase _ganttRow;
        private double _height;
        private int _bottomMargin;

        #endregion

        #region Properties
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                RaisePropertyChanged(nameof(Caption));
            }
        }
        public virtual int StartPosition
        {
            get
            {
                return _startPosition;
            }
            set
            {
                if (value <= 0)
                    _startPosition = 0;
                else
                    _startPosition = value;
                RaisePropertyChanged(nameof(StartPosition));
            }
        }
        public virtual int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                    if(_isSelected)
                        IsSelectedChanged(this);
                }
            }
        }
        public virtual int ScaleStep
        {
            get
            {
                return _scaleStep;
            }
            set
            {
                if (_scaleStep != value)
                {
                    _scaleStep = value;
                    RaisePropertyChanged(nameof(ScaleStep));
                }
            }
        }
        public GanttRowViewModelBase GanttRow
        {
            get
            {
                return _ganttRow;
            }
            set
            {
                _ganttRow = value;
                RaisePropertyChanged(nameof(GanttRow));
            }
        }
        public object Content { get; set; }
        public GanttItemInRowPosition InRowPosition { get; set; } = GanttItemInRowPosition.FullRow;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }
        public int BottomMargin
        {
            get => _bottomMargin;
            set
            {
                _bottomMargin = value;
                RaisePropertyChanged(nameof(BottomMargin));
            }
        }
        #endregion

        #region Events

        internal event IsSelectedDelegate IsSelectedChanged = delegate { };

        #endregion

        public GanttItemViewModelBase(GanttRowViewModelBase parentRow, GanttItemInRowPosition inRowPosition = GanttItemInRowPosition.FullRow)
        {
            _scaleStep = parentRow.GanttDiagram.ScaleStep;
            parentRow.GanttDiagram.ScaleStepChanged += GraphBase_ScaleStepChanged;
            GanttRow = parentRow;
            InRowPosition = inRowPosition;
            GanttRow.IsRowShrinkedChanged += GanttRow_IsRowShrinkedChanged;

            GanttRow_IsRowShrinkedChanged(GanttRow.IsShrinked);
        }

        #region Methods

        public virtual void DeleteItem(object obj)
        {
            _ganttRow.Items.Remove(this);
            if (_ganttRow.Items.Count == 0)
            {
                _ganttRow.GanttDiagram.Rows.Remove(_ganttRow);
            }
        }
        protected virtual void AddItem(object obj)
        {
            _ganttRow.AddItemCommand.Execute(null);
        }
        protected virtual void CopyItem(object obj)
        {
            _ganttRow.Items.Add(new GanttItemViewModelBase(_ganttRow));
        }

        public virtual void RecalculatePosition()
        {
        }

        #endregion

        #region Event handlers

        private void GraphBase_ScaleStepChanged(object sender, EventArgs e)
        {
            ScaleStep = _ganttRow.GanttDiagram.ScaleStep;
        }

        private void GanttRow_IsRowShrinkedChanged(bool isShrinked)
        {
            if (isShrinked)
            {
                Height = InRowPosition == GanttItemInRowPosition.FullRow ? 40 : 20;
                BottomMargin = InRowPosition == GanttItemInRowPosition.UpperHalf ? 20 : 0;
            }
            else
            {
                Height = InRowPosition == GanttItemInRowPosition.FullRow ? 80 : 40;
                BottomMargin = InRowPosition == GanttItemInRowPosition.UpperHalf ? 40 : 0;
            }
        }

        #endregion
    }
}
