using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfControlsLibrary.GanttDiagram.ViewModels;

namespace WpfControlsLibrary.GanttDiagram
{
    /// <summary>
    /// Логика взаимодействия для uscGanttItem.xaml
    /// </summary>
    public partial class uscGanttItem : UserControl
    {
        private bool _isMoving;
        private bool _isClicked;
        private double _mouseXPosLocal = 0;

        #region CanEdit
        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(uscGanttItem), new PropertyMetadata(false,
                new PropertyChangedCallback(
                    (d, e) => { ((uscGanttItem)d).OnCanEditChanged((bool)e.OldValue, (bool)e.NewValue); })));

        private void OnCanEditChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                this.MouseMove += UserControl_MouseMove;
                this.MouseUp += UserControl_MouseUp;
            }
            else
            {
                this.MouseMove -= UserControl_MouseMove;
                this.MouseUp -= UserControl_MouseUp;
            }
        }
        #endregion


        public uscGanttItem()
        {
            InitializeComponent();
        }

        private void UscGanttItem_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isClicked = true;

            if (CanEdit == true && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                _mouseXPosLocal = e.GetPosition((IInputElement)sender).X;
                _isMoving = true;
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMoving)
            {
                FrameworkElement element = sender as FrameworkElement;

                GanttItemViewModelBase vm = (element.DataContext as GanttItemViewModelBase);

                if (!(element.TemplatedParent is FrameworkElement contentPresenter))
                    return;

                int newPos = (int)(e.GetPosition(contentPresenter).X - _mouseXPosLocal);
                if (newPos <= vm.ScaleStep * 99 && Math.Abs(vm.StartPosition - newPos) > 5)
                    vm.StartPosition = newPos;
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMoving = false;
            _isClicked = false;
        }

        private void UscGanttItem_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isClicked && DataContext is GanttItemViewModelBase ganttItemViewModel)
            {
                ganttItemViewModel.IsSelected = true;
            }

            _isMoving = false;
            _isClicked = false;
        }

        private void UscGanttItem_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isMoving = false;
            _isClicked = false;
        }

        private void LeftAdorner_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _isMoving = false;
            if (!(this.Margin.Left == 0 && e.HorizontalChange < 0) && (this.Width - e.HorizontalChange) > 20)
            {
                this.Margin = new Thickness(this.Margin.Left + e.HorizontalChange, this.Margin.Top, this.Margin.Right, this.Margin.Bottom);
                this.Width -= e.HorizontalChange;
            }

            e.Handled = true;
        }

        private void RightAdorner_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _isMoving = false;
            FrameworkElement element = sender as FrameworkElement;
            GanttItemViewModelBase vm = (element.DataContext as GanttItemViewModelBase);
            if (this.Width + e.HorizontalChange > 20 && (vm.StartPosition + vm.Duration + this.Width + e.HorizontalChange) <= vm.ScaleStep * 100)
                this.Width += e.HorizontalChange;

            e.Handled = true;
        }
    }
}
