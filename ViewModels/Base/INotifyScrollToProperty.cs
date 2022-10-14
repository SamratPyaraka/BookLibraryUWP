
namespace BookLibrary1.ViewModels.Base
{
    public interface INotifyScrollToProperty
    {
        event ScrollToPropertyHandler ScrollToProperty;
    }

    public delegate void ScrollToPropertyHandler(string PropertyName);
}
