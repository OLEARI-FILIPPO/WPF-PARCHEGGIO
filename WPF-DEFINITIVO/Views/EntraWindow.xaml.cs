using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per EntraWindow.xaml
    /// </summary>
    public partial class EntraWindow : Window, INotifyPropertyChanged
    {
        EntraWindowViewModel creazione;
        public EntraWindow(EntraWindowViewModel viewModel)
        {
            InitializeComponent();
            creazione = viewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void entra_Click(object sender, RoutedEventArgs e)
        {
            //Se non c'è già un altra macchina allora proseguo

            //Inserisco il veicolo con la entry time date
            /* PARKING
             * [ID] [bigint] IDENTITY(1, 1) NOT NULL,
 
             [ParkingId] [char](3) NOT NULL,
 
             [Stato] [bit] NOT NULL,
 
             [Revenue] [decimal](18, 0) NULL,
	        [EntryTimeDate] [datetime] NULL,
	        [VehicleId] [bigint] NULL,
	        [InfoParkId] [bigint] NOT NULL,

            [Token] [char](5) NULL,*/

            //Query inserimento persona

            using (var client = new HttpClient())
            {
                OwnerVehicle parametri = new OwnerVehicle()
                {
                    Surname = creazione.Surname,
                    Name = creazione.Name,
                    DateBirth = creazione.DateBirth
                };

                //Chiamo l'api per la creazione del parcheggio
                var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/parcheggio", parametri);

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("PARCHEGGIO AGGIUNTO");
                }
                else
                {
                    MessageBox.Show("Problema durante l'inserimento.");
                }
            }

            //Query inserimento veicolo


        }
    }
}
