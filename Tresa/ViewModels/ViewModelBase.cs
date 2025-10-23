namespace Tresa.ViewModels
{
    public abstract class ViewModelBase
    {
        protected Task NavigateAsync(string route) =>
            MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync(route));
    }
}
