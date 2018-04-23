using System;
using System.ComponentModel;
using System.Windows;

namespace WpfControlsLibrary.Infrastrucrure.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static void AddValueChanged<T>(this T source, DependencyProperty property, EventHandler handler) 
            where T : DependencyObject
        {
            var desc = DependencyPropertyDescriptor.FromProperty(property, typeof(T));
            desc.AddValueChanged(source, handler);
        }

        public static void RemoveValueChanged<T>(this T source, DependencyProperty property, EventHandler handler)
        {
            var desc = DependencyPropertyDescriptor.FromProperty(property, typeof(T));
            desc.RemoveValueChanged(source, handler);
        }
    }
}
