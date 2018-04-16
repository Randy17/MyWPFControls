using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WpfControlsLibrary.CustomizableContextMenu.Configuration;
using WpfControlsLibrary.CustomizableContextMenu.Models;
using WpfControlsLibrary.CustomizableContextMenu.ViewModels;
using WpfControlsLibrary.Infrastrucrure;

namespace WpfControlsLibrary.CustomizableContextMenu
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlsLibrary.CustomizableContextMenu"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlsLibrary.CustomizableContextMenu;assembly=WpfControlsLibrary.CustomizableContextMenu"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CustomizableContextMenu : ContextMenu
    {
        private const double MAX_EXTENDED_LIST_HEIGHT = 200;

        private bool _isClosedFired = true;
        private uscContextMenuBody _body;
        private FrameworkElement _placementTarget;
        private double _currentVerticalOffset;
        private double _currentHorizontalOffset;

        private uscContextMenuBody Body
        {
            get
            {
                if(_body == null)
                {
                    _body = UIElementsTreeHelper.FindChild<uscContextMenuBody>(this);
                }
                return _body;
            }           
        }

        static CustomizableContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizableContextMenu), new FrameworkPropertyMetadata(typeof(CustomizableContextMenu)));

            
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.Opened += CustomizableContextMenu_Opened;
            this.Closed += CustomizableContextMenu_Closed;

            var isOpenPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(ContextMenu.IsOpenProperty, typeof(ContextMenu));
            isOpenPropertyDescriptor.AddValueChanged(this, OnIsOpenChanged);
        }        

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (_placementTarget == null)
            {
                _placementTarget = this.PlacementTarget as FrameworkElement;
                if (_placementTarget != null)
                    _placementTarget.ContextMenuOpening += PlacementTarget_ContextMenuOpening;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Popup myParentPopup = this.Parent as Popup;
            if (myParentPopup != null)
                myParentPopup.PopupAnimation = PopupAnimation.None;
        }

        #region DependencyProperties
        #region CommandsGroups
        public IList CommandsGroups
        {
            get { return (IList)GetValue(CommandsGroupsProperty); }
            set { SetValue(CommandsGroupsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandsGroups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandsGroupsProperty =
            DependencyProperty.Register("CommandsGroups", typeof(IList), typeof(CustomizableContextMenu), new PropertyMetadata(new List<ContextMenuGroup>(), (d, e) =>
            {
                CustomizableContextMenu menu = d as CustomizableContextMenu;
                if (menu != null)
                {
                    if (e.NewValue != e.OldValue)
                    {
                        menu.CommandsGroupsPropertyChanged(e.NewValue as List<ContextMenuGroup>);
                    }
                }
            }));
        #endregion

        #region MenuViewModel
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object MenuViewModel
        {
            get { return (object)GetValue(MenuViewModelProperty); }
            set { SetValue(MenuViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuViewModelProperty =
            DependencyProperty.Register("MenuViewModel", typeof(object), typeof(CustomizableContextMenu), new PropertyMetadata(null));
        #endregion

        #region MenuConfiguration
        public ContextMenuConfiguration MenuConfiguration
        {
            get { return (ContextMenuConfiguration)GetValue(MenuConfigurationProperty); }
            set { SetValue(MenuConfigurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuConfiguration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuConfigurationProperty =
            DependencyProperty.Register("MenuConfiguration", typeof(ContextMenuConfiguration), typeof(CustomizableContextMenu), new PropertyMetadata(null, (d, e) =>
            {
                CustomizableContextMenu menu = d as CustomizableContextMenu;
                if (menu != null)
                {
                    if (e.NewValue != e.OldValue)
                    {
                        menu.MenuConfigurationPropertyChanged(e.NewValue as ContextMenuConfiguration);
                    }
                }
            }));
        #endregion

        #region MaxExtendedListHeight
        [EditorBrowsable(EditorBrowsableState.Never)]
        public double MaxExtendedListHeight
        {
            get { return (double)GetValue(MaxExtendedListHeightProperty); }
            set { SetValue(MaxExtendedListHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxExtendedListHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxExtendedListHeightProperty =
            DependencyProperty.Register("MaxExtendedListHeight", typeof(double), typeof(CustomizableContextMenu), new PropertyMetadata(MAX_EXTENDED_LIST_HEIGHT));
        #endregion

        #region IsExtendedOpensOnTop
        public bool IsExtendedOpensOnTop
        {
            get { return (bool)GetValue(IsExtendedOpensOnTopProperty); }
            set { SetValue(IsExtendedOpensOnTopProperty, value); }
        }

        public double CurrentHorizontalOffset { get => _currentHorizontalOffset; set => _currentHorizontalOffset = value; }

        // Using a DependencyProperty as the backing store for IsExtendedOpensOnTop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExtendedOpensOnTopProperty =
            DependencyProperty.Register("IsExtendedOpensOnTop", typeof(bool), typeof(CustomizableContextMenu), new PropertyMetadata(false));
        #endregion
        #endregion

        #region EventHandlers
        private void CommandsGroupsPropertyChanged(List<ContextMenuGroup> commandsGroups)
        {
            MenuViewModel = new MainViewModel(commandsGroups);
            if (MenuConfiguration != null)
            {
                (MenuViewModel as MainViewModel).LoadConfiguration(MenuConfiguration);
            }
        }
        private void MenuConfigurationPropertyChanged(ContextMenuConfiguration configuration)
        {
            if (MenuViewModel is MainViewModel)
            {
                (MenuViewModel as MainViewModel).LoadConfiguration(configuration);
            }
        }
        private void OnIsOpenChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("IsOpen =" + this.IsOpen);
            if (this.IsOpen == false && _isClosedFired == false)
            {
                ResetContextMenuState();
            }
        }
        private void PlacementTarget_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Opening.");
            
            if (_isClosedFired == false)
            {
                System.Diagnostics.Debug.WriteLine("==========================> Error. Menu was not closed.");
                //ResetContextMenuState();
            }
        }
        private void CustomizableContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Opened.");

            _currentVerticalOffset = Body.grdExtendBtn.ActualHeight / -2;
            CurrentHorizontalOffset = Body.grdExtendBtn.ActualWidth / -2;
            RecalculateContextMenuPosition();

            this.VerticalOffset = _currentVerticalOffset;
            this.HorizontalOffset = CurrentHorizontalOffset;
            
            _isClosedFired = false;            
        }
        private void CustomizableContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Closed.");
            ResetContextMenuState();
            _isClosedFired = true;            
        }
        #endregion

        #region Methods
        private void RecalculateContextMenuPosition()
        {
            //FrameworkElement placementTarget = this.PlacementTarget as FrameworkElement;
            //Point menuPosition = this.TranslatePoint(new Point(0, 0), placementTarget);
            Point mousePosition = Mouse.GetPosition(_placementTarget);
            Point menuPosition = new Point(mousePosition.X, mousePosition.Y);
            menuPosition.Y += _currentVerticalOffset;
            menuPosition.X += CurrentHorizontalOffset;

            double verticalDiff = (_placementTarget.DesiredSize.Height - (menuPosition.Y + this.ActualHeight));
            if (verticalDiff < -MaxExtendedListHeight)
            {
                IsExtendedOpensOnTop = true;
                _currentVerticalOffset = _placementTarget.DesiredSize.Height - mousePosition.Y;
            }
            else if (verticalDiff < 0)
            {
                IsExtendedOpensOnTop = true;
                _currentVerticalOffset -= MaxExtendedListHeight;
            }

            double horizontalDiff = (_placementTarget.DesiredSize.Width - (menuPosition.X + this.ActualWidth));
            if (horizontalDiff < 0)
            {
                CurrentHorizontalOffset = _placementTarget.DesiredSize.Width - mousePosition.X;
            }
        }
        private void ResetContextMenuState()
        {
            IsExtendedOpensOnTop = false;
            _currentVerticalOffset = 0;
            CurrentHorizontalOffset = 0;
        }
        #endregion
    }
}
