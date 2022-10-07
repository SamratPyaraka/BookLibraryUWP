

using BookLibrary1.Services.RequestService;
using BookLibrary1.Services;
using BookLibrary1.Views;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using BookLibrary1.Helpers;

namespace BookLibrary1.ViewModels
{
    public class LoginDetailsViewModel : ViewModelBase
    {
        public LoginDetailsViewModel()
        {
            
        }

        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                var _requestService = Locator.Instance.Resolve<IRequestService>();
                if (await _requestService.IsAccessTokenValid())
                {
                    await Task.Delay(2000);
                    NavigationService.Navigate(typeof(ShellPage));
                }
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsViewModel->Initialize");
            }

        }

    }
}
