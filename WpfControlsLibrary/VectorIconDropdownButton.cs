using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlsLibrary
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlsLibrary"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlsLibrary;assembly=WpfControlsLibrary"
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
    ///     <MyNamespace:VectorIconDropdownButton/>
    ///
    /// </summary>
    [TemplatePart(Name = PART_DropDownButton, Type = typeof(VectorIconToggleButton))]
    [TemplatePart(Name = PART_ContentPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
    public class VectorIconDropdownButton : ContentControl
    {
        private const string PART_DropDownButton = "PART_DropDownButton";
        private const string PART_ContentPresenter = "PART_ContentPresenter";
        private const string PART_Popup = "PART_Popup";

        private VectorIconToggleButton _button;
        private Popup _popup;
        private ContentPresenter _contentPresenter;

        static VectorIconDropdownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VectorIconDropdownButton), new FrameworkPropertyMetadata(typeof(VectorIconDropdownButton)));
        }

        public VectorIconDropdownButton()
        {
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
        }

        #region DependencyProperties

        public string VectorIconPath
        {
            get { return (string)GetValue(VectorIconPathProperty); }
            set { SetValue(VectorIconPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VectorIconPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VectorIconPathProperty =
            DependencyProperty.Register("VectorIconPath", typeof(string), typeof(VectorIconDropdownButton), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(VectorIconDropdownButton), new PropertyMetadata(null));

        public Brush IconFillBrush
        {
            get { return (Brush)GetValue(IconFillBrushProperty); }
            set { SetValue(IconFillBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconFillBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFillBrushProperty =
            DependencyProperty.Register("IconFillBrush", typeof(Brush), typeof(VectorIconDropdownButton), new PropertyMetadata(null));



        public PlacementMode DropDownPosition   
        {
            get { return (PlacementMode)GetValue(DropDownPositionProperty); }
            set { SetValue(DropDownPositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropDownPositionProperty =
            DependencyProperty.Register("DropDownPosition", typeof(PlacementMode), typeof(VectorIconDropdownButton), new PropertyMetadata(PlacementMode.Bottom));




        public Brush DropDownContentBackground
        {
            get { return (Brush)GetValue(DropDownContentBackgroundProperty); }
            set { SetValue(DropDownContentBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropDownContentBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropDownContentBackgroundProperty =
            DependencyProperty.Register("DropDownContentBackground", typeof(Brush), typeof(VectorIconDropdownButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));



        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDropDownHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(VectorIconDropdownButton), new PropertyMetadata(SystemParameters.PrimaryScreenHeight / 2.0));



        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpened.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(VectorIconDropdownButton), new PropertyMetadata(false));



        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _button = GetTemplateChild(PART_DropDownButton) as VectorIconToggleButton;
            if (_popup != null)
                _popup.Opened -= Popup_Opened;

            _popup = GetTemplateChild(PART_Popup) as Popup;

            if (_popup != null)
                _popup.Opened += Popup_Opened;
        }

        private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            CloseDropDown(true);
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            // Set the focus on the content of the ContentPresenter.
            if (_contentPresenter != null)
            {
                _contentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }
        }



        private void CloseDropDown(bool isFocusOnButton)
        {
            if (IsOpened)
            {
                IsOpened = false;
            }
            ReleaseMouseCapture();

            if (isFocusOnButton && (_button != null))
            {
                _button.Focus();
            }
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
            if (!(bool)e.NewValue)
            {
                this.CloseDropDown(false);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (_button != null)
            {
                _button.Focus();
            }
        }
    }
}
