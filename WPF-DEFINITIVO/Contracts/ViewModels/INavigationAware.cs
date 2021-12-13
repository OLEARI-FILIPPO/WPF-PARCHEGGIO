using System.Threading.Tasks;

namespace WPF_DEFINITIVO.Contracts.ViewModels
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object parameter);

        void OnNavigatedFrom();
    }
}
