﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfControlsLibrary.Infrastrucrure
{
    public class UIElementsTreeHelper
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            if (childCount == 0)
                return null;

            for(int i = 0; i < childCount; i++)
            {
                T child = VisualTreeHelper.GetChild(parent, i) as T;

                if (child != null)
                    return child;
                else
                    return FindChild<T>(child);
            }

            return null;
        }
    }
}
