using BookLibrary1.Helpers;
using BookLibrary1.Services;
using BookLibrary1.Views;
using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using static Google.Apis.Requests.BatchRequest;


namespace BookLibrary1.ViewModels
{
    public class LogoutViewModel : ViewModelBase
    {
        public LogoutViewModel()
        {
        }
        public async void Initialize(object frame, IList<KeyboardAccelerator> keyboardAccelerators)
        {

            var res = new MessageDialog("Are you sure you want to logout?", "");
            res.Commands.Add(new UICommand("Yes"));
            res.Commands.Add(new UICommand("No"));
            // Set the command that will be invoked by default
            res.DefaultCommandIndex = 0;
            res.CancelCommandIndex = 1;

            // Show the message dialog
            var x = await res.ShowAsync();
            string getUserInput = x.Label;
            if (getUserInput == "No")
                NavigationService.Navigate(typeof(BooksStorePage));
            else {

                AppSettings.GAccessTokenID = "";
                AppSettings.Account = null;
                AppSettings.IDTokenPayLoad = null;
                // NavHelper.LogoutUser();
                NavigationService.Navigate(typeof(LoginDetailsPage));
            }
        }

    }
}
