using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvengersUtd.BrickLab.Controls
{
    public class TextBoxTemplateColumn : DataGridTemplateColumn
    {
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            var contentPresenter = editingElement as ContentPresenter;

            var editingControl = FindVisualChild<TextBox>(contentPresenter);
            if (editingControl == null)
                return null;

            editingControl.Focus();
            editingControl.SelectAll();
            return null;
        }

        private static TChildItem FindVisualChild<TChildItem>(DependencyObject obj)
            where TChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TChildItem)
                    return (TChildItem)child;
                else
                {
                    TChildItem childOfChild = FindVisualChild<TChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
