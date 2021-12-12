using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Contracts.ViewModels;
using WPF_DEFINITIVO.Core.Contracts.Services;
using WPF_DEFINITIVO.Core.Models;
using WPF_DEFINITIVO.Helpers;

namespace WPF_DEFINITIVO.ViewModels
{
    public class MainViewModel : ObservableObject, INavigationAware
    {
        public MainViewModel()
        {


        }
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<Parking> Source { get; } = new ObservableCollection<Parking>();

        public MainViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public ObservableCollection<Parking> ParkingObject;
        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();


            // Replace this with your actual data
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/NotParked");

                var data = await response.Content.ReadAsStringAsync();

                ParkingObject = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(data);

                foreach (Parking item in ParkingObject)
                {
                    Source.Add(item);
                }
            }


        }

        public void OnNavigatedFrom()
        {
        }
    }
}
