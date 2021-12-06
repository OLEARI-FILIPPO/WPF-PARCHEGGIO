using System.Collections.Generic;
using System.Threading.Tasks;

using WPF_DEFINITIVO.Core.Models;

namespace WPF_DEFINITIVO.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();

        Task<IEnumerable<SampleOrder>> GetGridDataAsync();
    }
}
