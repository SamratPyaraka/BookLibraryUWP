using System;
using BookLibrary1.Services;
using BookLibrary1.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class UserAccountInfoPage : Page
    {
        public UserAccountInfoViewModel ViewModel { get; } = new UserAccountInfoViewModel();

        public UserAccountInfoPage()
        {
            InitializeComponent();
            accountDetails.Visibility = Visibility.Visible;
            billingDetails.Visibility = Visibility.Collapsed;
            trackOrderDetails.Visibility = Visibility.Collapsed;
            orderDetails.Visibility = Visibility.Collapsed;
        }

        private void AccountDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            accountDetails.Visibility = Visibility.Visible;
            billingDetails.Visibility = Visibility.Collapsed;
            trackOrderDetails.Visibility = Visibility.Collapsed;
            orderDetails.Visibility = Visibility.Collapsed;
        }

        private void BillingDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            accountDetails.Visibility = Visibility.Collapsed;
            billingDetails.Visibility = Visibility.Visible;
            trackOrderDetails.Visibility = Visibility.Collapsed;
            orderDetails.Visibility = Visibility.Collapsed;
        }

        private void DeliveryDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            accountDetails.Visibility = Visibility.Collapsed;
            billingDetails.Visibility = Visibility.Collapsed;
            trackOrderDetails.Visibility = Visibility.Visible;
            orderDetails.Visibility = Visibility.Collapsed;
        }

        private void TrackOrderDetails_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            accountDetails.Visibility = Visibility.Collapsed;
            billingDetails.Visibility = Visibility.Collapsed;
            trackOrderDetails.Visibility = Visibility.Collapsed;
            orderDetails.Visibility = Visibility.Visible;
        }

        private void OwnedBorrowed_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(BookKeeperPage));
        } 
    }
}
