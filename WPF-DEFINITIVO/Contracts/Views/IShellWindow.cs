using System.Windows.Controls;

namespace WPF_DEFINITIVO.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
