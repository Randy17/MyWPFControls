using System.Collections.ObjectModel;
using System.Linq;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.GanttDiagram.ViewModels
{
    public class GanttRowViewModelBase : ViewModelBase
    {
        #region Fields
        protected GanttDiagramViewModelBase _ganttDiagram;
        private bool _isShrinked;

        private Command _moveRowUpCmd;
        private Command _moveRowDownCmd;
        #endregion

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
        public GanttDiagramViewModelBase GanttDiagram
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
                _isShrinked = value;
                RaisePropertyChanged(nameof(IsShrinked));
            }
        }
        #endregion

        #region Commands
        internal Command DeleteRowCommand
        {
            get
            {
                return new Command(DeleteRow);
            }
        }
        internal Command AddItemCommand
        {
            get
            {
                return new Command(AddItem);
            }
        }
        internal Command MoveRowUpCmd
        {
            get
            {
                if (_moveRowUpCmd == null)
                    _moveRowUpCmd = new Command(MoveRowUp);
                return _moveRowUpCmd;
            }
        }
        internal Command MoveRowDownCmd
        {
            get
            {
                if (_moveRowDownCmd == null)
                    _moveRowDownCmd = new Command(MoveRowDown);
                return _moveRowDownCmd;
            }
        }
        #endregion

        public GanttRowViewModelBase(string caption, GanttDiagramViewModelBase parentGraphBase, int position = 0)
        {
            Caption = caption;
            Items = new ObservableCollection<GanttItemViewModelBase>();
            _ganttDiagram = parentGraphBase;
            Position = position;
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
