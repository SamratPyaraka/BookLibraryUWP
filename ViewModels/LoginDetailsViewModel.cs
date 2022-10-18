

using BookLibrary1.Services.RequestService;
using BookLibrary1.Services;
using BookLibrary1.Views;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using BookLibrary1.Helpers;
using Windows.UI.Xaml.Controls;
using BookLibrary1.Services.UserService;
using System;

namespace BookLibrary1.ViewModels
{
    public class LoginDetailsViewModel : Page
    {
        public LoginDetailsViewModel()
        {
            
        }

        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                //var _requestService = Locator.Instance.Resolve<IRequestService>();
                //if (await _requestService.IsAccessTokenValid())
                //{
                //    var userServices = Locator.Instance.Resolve<IUserServices>();
                //    var res = await userServices.GetUserFromEmail(AppSettings.IDTokenPayLoad.Email);
                //    if (res.Response)
                //    {
                //        AppSettings.Account = res.Data;
                //        if(AppSettings.Account.AccountType != null)
                //        {
                //            NavigationService.Navigate(typeof(ShellPage), DateTime.Now.Ticks);
                //        }
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsViewModel->Initialize");
            }
            finally { }

        }

    }
}
