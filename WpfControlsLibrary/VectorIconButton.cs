using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlsLibrary.Members;

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
    ///     <MyNamespace:VectorIconButton/>
    ///
    /// </summary>
    public class VectorIconButton : Button
    {
        static VectorIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VectorIconButton), new FrameworkPropertyMetadata(typeof(VectorIconButton)));
        }

        #region DependencyProperties
        public string VectorIconPath
        {
            get { return (string)GetValue(VectorIconPathProperty); }
            set { SetValue(VectorIconPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VectorIconPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VectorIconPathProperty =
            DependencyProperty.Register("VectorIconPath", typeof(string), typeof(VectorIconButton), new PropertyMetadata(null, OnVectorIconPathChanged));


        public DrawingImage IconDrawingImage
        {
            get { return (DrawingImage)GetValue(IconDrawingImageProperty); }
            set { SetValue(IconDrawingImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconDrawingImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconDrawingImageProperty =
            DependencyProperty.Register("IconDrawingImage", typeof(DrawingImage), typeof(VectorIconButton), new PropertyMetadata(null, OnIconDrawingImageChanged));

        /// <summary>
        /// Property for background icon (white colored)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DrawingImage BlurIconDrawingImage
        {
            get { return (DrawingImage)GetValue(BlurIconDrawingImageProperty); }
            set { SetValue(BlurIconDrawingImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlurIconDrawingImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlurIconDrawingImageProperty =
            DependencyProperty.Register("BlurIconDrawingImage", typeof(DrawingImage), typeof(VectorIconButton), new PropertyMetadata(null));

        /// <summary>
        /// Property for icon in disabled mode (grey colored)
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DrawingImage DisabledIconDrawingImage
        {
            get { return (DrawingImage)GetValue(DisabledIconDrawingImageProperty); }
            set { SetValue(DisabledIconDrawingImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisabledIconDrawingImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisabledIconDrawingImageProperty =
            DependencyProperty.Register("DisabledIconDrawingImage", typeof(DrawingImage), typeof(VectorIconButton), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(VectorIconButton), new PropertyMetadata(null));

        public Brush IconFillBrush
        {
            get { return (Brush)GetValue(IconFillBrushProperty); }
            set { SetValue(IconFillBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconFillBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconFillBrushProperty =
            DependencyProperty.Register("IconFillBrush", typeof(Brush), typeof(VectorIconButton), new PropertyMetadata(null, (d, e) =>
            {
                VectorIconButton viButton = d as VectorIconButton;
                if (viButton != null)
                {
                    viButton.IconFillBrushChanged(e.NewValue as Brush);
                }
            }));
        #endregion

        private static void OnVectorIconPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VectorIconButton).ConvertSVGToXaml(e.NewValue as string);
        }
        private void ConvertSVGToXaml(string file)
        {
            if (!File.Exists(file) || System.IO.Path.GetExtension(file) != ".svg")
            {
                return;
            }

            if (IconDrawingImage != null)
            {
                IconDrawingImage = null;
                BlurIconDrawingImage = null;
                DisabledIconDrawingImage = null;
            }

            SvgConverter conv = new SvgConverter();
            DrawingGroup dg = conv.SvgFileToWpfObject(file);
            dg.RemoveWhiteGeometryDrawings();
            IconDrawingImage = new DrawingImage(dg);
        }

        private static void OnIconDrawingImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                VectorIconButton sender = (d as VectorIconButton);
                sender.ChangeIconDrawingImage(e.NewValue);
            }
        }

        private void ChangeIconDrawingImage(object newValue)
        {
            DrawingImage icon = newValue as DrawingImage;
            if (icon == null)
                return;



            DrawingGroup iconDrawingGroup = icon.Drawing as DrawingGroup;
            if (iconDrawingGroup == null)
                return;

            if (IconFillBrush != null)
            {
                iconDrawingGroup.PaintAllGemetryDrawings(IconFillBrush);
            }

            DrawingGroup whiteDg = iconDrawingGroup.Clone();
            whiteDg.PaintAllGemetryDrawings(Colors.White);
            DrawingGroup greyDg = iconDrawingGroup.Clone();
            greyDg.PaintAllGemetryDrawings(Colors.Gray);

            BlurIconDrawingImage = new DrawingImage(whiteDg);
            DisabledIconDrawingImage = new DrawingImage(greyDg);

        }

        private void IconFillBrushChanged(Brush brush)
        {
            if (brush != null && IconDrawingImage != null && (IconDrawingImage.Drawing is DrawingGroup))
            {
                (IconDrawingImage.Drawing as DrawingGroup).PaintAllGemetryDrawings(brush);
            }
        }

    }
}
