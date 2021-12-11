using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
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
    public class StoricoViewModel : ObservableObject, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<Parking> Source { get; set; } = new ObservableCollection<Parking>();
        public object ParkingObject { get; private set; }

        public StoricoViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/history");
                var list = await response.Content.ReadAsStringAsync();

                var lista = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(list);

                if (response.IsSuccessStatusCode)
                {
                    foreach (var item in lista)
                    {
                        Source.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Nessuna cronologia presente.");
                }
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
