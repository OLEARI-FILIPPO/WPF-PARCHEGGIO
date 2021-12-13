using System.Threading.Tasks;

namespace WPF_DEFINITIVO.Contracts.ViewModels
{
    public interface INavigationAware
    {
        Task OnNavigatedTo(object parameter);

        void OnNavigatedFrom();
    }
}
