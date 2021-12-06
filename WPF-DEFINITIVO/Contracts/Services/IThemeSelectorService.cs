using System;

using WPF_DEFINITIVO.Models;

namespace WPF_DEFINITIVO.Contracts.Services
{
    public interface IThemeSelectorService
    {
        void InitializeTheme();

        void SetTheme(AppTheme theme);

        AppTheme GetCurrentTheme();
    }
}
