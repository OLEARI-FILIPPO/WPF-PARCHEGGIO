using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using WPF_DEFINITIVO.Contracts.Services;
using WPF_DEFINITIVO.Contracts.ViewModels;
using WPF_DEFINITIVO.Core.Contracts.Services;
using WPF_DEFINITIVO.Core.Models;

namespace WPF_DEFINITIVO.ViewModels
{
    public class ParcheggiViewModel : ObservableObject//, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;
        private ICommand _navigateToDetailCommand;

        /*public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<SampleOrder>(NavigateToDetail));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ParcheggiViewModel(ISampleDataService sampleDataService, INavigationService navigationService)
        {
            _sampleDataService = sampleDataService;
            _navigationService = navigationService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        private void NavigateToDetail(SampleOrder order)
        {
            _navigationService.NavigateTo(typeof(ParcheggiDetailViewModel).FullName, order.OrderID);
        }*/


        public string parking;
        
        public string Parking
        {
            get { return this.parking; }
            set
            {
                this.parking = value;
                OnPropertyChanged("Token");
            }
        }
    }
}
