using BookLibrary1.Services.UserService;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace BookLibrary1.ViewModels
{
    public class ViewModelBase : ObservableObject, INotifyPropertyChanged
    {
        public IUserServices userServices;
        public ViewModelBase()
        {
            userServices = Locator.Instance.Resolve<IUserServices>();
        }
    }
}
