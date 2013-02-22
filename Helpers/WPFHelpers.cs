using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helpers
{
    public static class WPFHelpers
    {
		//public static void SwitchToNewWindow<TWindow>() where TWindow : Window, new()
		//{
		//	SwitchToNewWindow<TWindow>(null);
		//}

		//public static void SwitchToNewWindow<TWindow>(object[] args)  where TWindow : Window, new()
		//{
		//	Window windowToSwitchTo = new Window();
		//	if (args != null)
		//	{
		//		windowToSwitchTo = (Window)Activator.CreateInstance(typeof(TWindow), args);
		//		if (Equals(windowToSwitchTo, new Window()))
		//		{
		//			"Unable to create window of type: {0}, with arguments: [ {1} ]".ShowMessage(new object[] { typeof(TWindow), args });
		//		}
		//	}
		//	else
		//		windowToSwitchTo = new TWindow();
		//	Window windowToSwitchFrom = Application.Current.MainWindow;
		//	Application.Current.MainWindow = windowToSwitchTo;
		//	windowToSwitchFrom.Close();
		//	windowToSwitchTo.Show();
		//}

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        public static void SetPhillipForegroundColors(this Window window)
        {
            IEnumerable<Label> labels = Helpers.WPFHelpers.FindVisualChildren<Label>(window);
            foreach (Label label in labels)
            {
                label.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
        }

        public static void SetPhillipBackgroundColors( this Window window)
        {
            window.Background = new SolidColorBrush(Colors.Black);
            window.Foreground = new SolidColorBrush(Colors.Orange);
        }

        //// usage xPanel.SetBackground(SystemColors.DesktopBrushKey);
        //public static void SetBackground(this Panel panel, ResourceKey key)
        //{
        //    panel.SetResourceReference(Panel.BackgroundProperty, key);
        //}

        //// usage xControl.SetBackground(SystemColors.DesktopBrushKey);
        //public static void SetBackground(this Control control, ResourceKey key)
        //{
        //    control.SetResourceReference(Control.BackgroundProperty,);
        //}
        public static void SetPhillipForegroundColors(this UserControl control)
        {
            IEnumerable<Label> labels = Helpers.WPFHelpers.FindVisualChildren<Label>(control);
            foreach (Label label in labels)
            {
                label.Foreground = new SolidColorBrush(Colors.DarkOrange);
            }
        }
    }
}
