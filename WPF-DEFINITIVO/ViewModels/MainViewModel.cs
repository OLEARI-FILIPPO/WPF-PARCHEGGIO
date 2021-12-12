using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            GetNotParked();

        }

        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<Parking> Source { get { GetNotParked(); return parkings; } } 

        public MainViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        //public async void OnNavigatedTo(object parameter)
        //{
        //    Source.Clear();

        //    // Replace this with your actual data
        //    var data = await _sampleDataService.GetGridDataAsync();

        //    foreach (var item in data)
        //    {
        //        Source.Add(item);
        //    }
        //}

        private ObservableCollection<Parking> parkings;

        public async void GetNotParked()
        {
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/NotParked");

                var list = await response.Content.ReadAsStringAsync();

              //  MessageBox.Show(list.ToString());

                parkings = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(list);

                

            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
