using System.Threading.Tasks;

namespace WPF_DEFINITIVO.Contracts.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle();

        Task HandleAsync();
    }
}
