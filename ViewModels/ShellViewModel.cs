using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using BookLibrary1.Helpers;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace BookLibrary1.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _itemInvokedCommand;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));
        public ICommand OnUserIconClickCmd => new RelayCommand(NavigateToAccount);
        private void NavigateToAccount()
        {
            NavigationService.Navigate(typeof(UserAccountInfoPage));
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public ShellViewModel()
        {
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set {
                 SetProperty(ref _UserName, value); OnPropertyChanged(); }
        }

        private string _UserIconUrl;

        public string UserIconUrl
        {
            get { return _UserIconUrl; }
            set { _UserIconUrl=value; OnPropertyChanged(); }
        }

        private Visibility _IsAdmin;

        public Visibility IsAdmin
        {
            get { return _IsAdmin; }
            set { SetProperty(ref _IsAdmin , value); OnPropertyChanged(); }
        }

        private bool _IsNavMenuVisible;

        public bool IsNavMenuVisible
        {
            get { return _IsNavMenuVisible; }
            set { SetProperty(ref _IsNavMenuVisible, value); OnPropertyChanged(); }
        }

        private Visibility _IsProfileIconVisible;

        public Visibility IsProfileIconVisible
        {
            get { return _IsProfileIconVisible; }
            set { SetProperty(ref _IsProfileIconVisible, value); OnPropertyChanged(); }
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                _navigationView = navigationView;
                _keyboardAccelerators = keyboardAccelerators;
                NavigationService.Frame = frame;
                NavigationService.NavigationFailed += Frame_NavigationFailed;
                NavigationService.Navigated += Frame_Navigated;
                _navigationView.BackRequested += OnBackRequested;

                UserIconUrl = AppSettings.IDTokenPayLoad?.Picture;
                UserName = "Welcome " + AppSettings.IDTokenPayLoad?.GivenName;
                IsAdmin = AppSettings.Account?.AccountType == Model.AccountType.Admin ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "Shell->Init");
            }
             
        }

        private async void OnLoaded()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
            await Task.CompletedTask;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // Navigate to the settings page - implement as appropriate if needed
            }
            else
            {
                var selectedItem = args.InvokedItemContainer as WinUI.NavigationViewItem;
                var pageType = selectedItem?.GetValue(NavHelper.NavigateToProperty) as Type;

                if (pageType != null)
                {
                    NavigationService.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
                }
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            UserIconUrl = AppSettings.IDTokenPayLoad?.Picture;
            UserName = "Welcome " + AppSettings.IDTokenPayLoad?.GivenName;
            IsAdmin = AppSettings.Account?.AccountType == Model.AccountType.Admin ? Visibility.Visible : Visibility.Collapsed;
            if (e.SourcePageType.Name == "LoginDetailsPage" || e.SourcePageType.Name == "RegistrationPage")
            {
                IsNavMenuVisible = false;
                //IsProfileIconVisible = Visibility.Collapsed;
            }
            else
            {
                IsNavMenuVisible = true;
                //IsProfileIconVisible = Visibility.Visible;
            }
            var selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
