﻿using System.Collections.ObjectModel;
using System.Linq;
using WpfControlsLibrary.GanttDiagram.ViewModels.Interfaces;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    internal delegate void IsRowShrinkedChanged(bool isShrinked);

    internal class GanttRowViewModelBase : ViewModelBase
    {
        #region Fields
        protected IGanttDiagramViewModel _ganttDiagram;
        private string _addItemToolTip;
        private string _deleteRowToolTip;
        private bool _isShrinked;
        private double _height;

        private Command _moveRowUpCmd;
        private Command _moveRowDownCmd;        
        #endregion

        internal event IsRowShrinkedChanged IsRowShrinkedChanged = delegate { };

        #region Properties
        public string Caption
        {
            get;
            set;
        }
        public int Position
        {
            get;
            set;
        }
        public string AddItemToolTip
        {
            get
            {
                return _addItemToolTip;
            }
            set
            {
                _addItemToolTip = value;
                RaisePropertyChanged(nameof(AddItemToolTip));
            }
        }
        public string DeleteRowToolTip
        {
            get
            {
                return _deleteRowToolTip;
            }
            set
            {
                _deleteRowToolTip = value;
                RaisePropertyChanged(nameof(DeleteRowToolTip));
            }
        }
        public IGanttDiagramViewModel GanttDiagram
        {
            get
            {
                return _ganttDiagram;
            }
        }
        public ObservableCollection<GanttItemViewModelBase> Items
        {
            get;
            set;
        }
        public bool IsShrinked
        {
            get
            {
                return _isShrinked;
            }
            set
            {
                if (_isShrinked != value)
                {
                    _isShrinked = value;
                    RaisePropertyChanged(nameof(IsShrinked));
                    IsRowShrinkedChanged(_isShrinked);
                    Height = IsShrinked ? 50 : 100;
                }
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }
        #endregion

        #region Commands
        public Command DeleteRowCommand
        {
            get
            {
                return new Command(DeleteRow);
            }
        }
        public Command AddItemCommand
        {
            get
            {
                return new Command(AddItem);
            }
        }
        public Command MoveRowUpCmd
        {
            get
            {
                if (_moveRowUpCmd == null)
                    _moveRowUpCmd = new Command(MoveRowUp);
                return _moveRowUpCmd;
            }
        }
        public Command MoveRowDownCmd
        {
            get
            {
                if (_moveRowDownCmd == null)
                    _moveRowDownCmd = new Command(MoveRowDown);
                return _moveRowDownCmd;
            }
        }
        #endregion

        public GanttRowViewModelBase(string caption, IGanttDiagramViewModel parentGraphBase, int position = 0)
        {
            Caption = caption;
            Items = new ObservableCollection<GanttItemViewModelBase>();
            _ganttDiagram = parentGraphBase;
            Position = position;

            Height = IsShrinked ? 50 : 100;
        }

        #region Methods
        protected virtual void DeleteRow(object obj)
        {
            _ganttDiagram.Rows.Remove(this);
        }
        protected virtual void AddItem(object obj)
        {
            Items.Add(new GanttItemViewModelBase(this));
        }
        protected virtual void MoveRowUp(object obj)
        {
            if (Position == 1)
                return;

            GanttRowViewModelBase row = _ganttDiagram.Rows.FirstOrDefault(r => r.Position == Position - 1);
            if (row != null)
            {
                row.Position = Position;
                _ganttDiagram.Rows.Move(Position - 1, Position - 2);
                Position = Position - 1;
            }
        }
        protected virtual void MoveRowDown(object obj)
        {
            if (Position == _ganttDiagram.Rows.Count)
                return;

            GanttRowViewModelBase row = _ganttDiagram.Rows.FirstOrDefault(r => r.Position == Position + 1);
            if (row != null)
            {
                row.Position = Position;
                _ganttDiagram.Rows.Move(Position - 1, Position);
                Position = Position + 1;
            }
        }
        #endregion
    }
}
