using BookLibrary1.Services.UserService;
using BookLibrary1.ViewModels.Base;
using System.ComponentModel;

namespace BookLibrary1.ViewModels
{
    public class ViewModelBase : ValidationBase, INotifyPropertyChanged
    {
        public IUserServices userServices;
        public ViewModelBase()
        {
            userServices = Locator.Instance.Resolve<IUserServices>();
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); }
        }

    }
}
