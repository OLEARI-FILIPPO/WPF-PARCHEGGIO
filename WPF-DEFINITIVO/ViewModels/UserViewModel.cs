using Microsoft.Toolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;

namespace WPF_DEFINITIVO.ViewModels
{
    public class UserViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Username;
        public string Password;
        public string Token;

        public UserViewModel(Users user, string result)
        {
            Username = user.Username;
            Password = user.Password;
            Token = result;
        }

        public string lastLogin;
        public string lastLogout;
        public string LastLogout
        {
            get { return lastLogout; }
            set
            {
                lastLogout = value;
                OnPropertyChanged("lastLogout");
            }
        }

        public string LastLogin
        {
            get { return lastLogin; }
            set
            {
                lastLogin = value;
                OnPropertyChanged("lastLogin");
            }
        }

        public async Task GetLast()
        {
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    string url = "http://localhost:13636/api/v1/GetLogin" + "/" + NavigationLoginToLogout._user.Username + "/" + NavigationLoginToLogout._user.Password;
                    var response = await client.GetAsync(url);
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();

                    Users u = JsonConvert.DeserializeObject<Users>(list);

                    //List<string> Parkings = new List<string>();

                    if (response.IsSuccessStatusCode)
                    {
                        lastLogin = u.LastLogin.ToString();
                        lastLogout = u.LastLogout.ToString();

                    }
                    else
                    {
                        MessageBox.Show(list);
                    }



                    // var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                }
            }
        }
    }
}
