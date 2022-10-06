using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Xamarin.Essentials;

namespace BookLibrary1.ViewModels
{
    public class LoginDetailsViewModel : ViewModelBase
    {
        public LoginDetailsViewModel()
        {
        }

        string accessToken = string.Empty;

        public string AuthToken
        {
            get => accessToken;
            set => SetProperty(ref accessToken, value);
        }

        public ICommand GoogleCommand => new RelayCommand(GoogleLogin);

        public async void GoogleLogin()
        {
            try
            {
                WebAuthenticatorResult r = null;

                var authUrl = new Uri(AppSettings.DefaultEndpoint+"/api/Auth/" + "Google");
                var callbackUrl = new Uri("uwp.books.library://");

                r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);


                AuthToken = r?.AccessToken ?? r?.IdToken;
            }
            catch (Exception ex)
            {
                AuthToken = string.Empty;
                //await DisplayAlertAsync($"Failed: {ex.Message}");
            }
        }

    }
}
