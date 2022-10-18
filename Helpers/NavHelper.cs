using System;
using BookLibrary1.Views;
using Microsoft.UI.Xaml.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;

namespace BookLibrary1.Helpers
{
    public class NavHelper
    {
        // This helper class allows to specify the page that will be shown when you click on a NavigationViewItem
        //
        // Usage in xaml:
        // <winui:NavigationViewItem x:Uid="Shell_Main" Icon="Document" helpers:NavHelper.NavigateTo="views:MainPage" />
        //
        // Usage in code:
        // NavHelper.SetNavigateTo(navigationViewItem, typeof(MainPage));
        public static Type GetNavigateTo(NavigationViewItem item)
        {
            return (Type)item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(NavigationViewItem item, Type value)
        {
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(Type), typeof(NavHelper), new PropertyMetadata(null));

        public static void LogoutUser(Uri uri=null)
        {
            // Gets the current frame, making one if needed.
            var sframe = Window.Current.Content as Frame;
            if (sframe == null)
                sframe = new Frame();

            // Opens the URI for "navigation" (handling) on the MainPage.
            sframe.Navigate(typeof(LoginDetailsPage), uri);
            sframe.BackStack.Clear();
            Window.Current.Content = sframe;
            //Window.Current.Activate();
        }
    }
}
