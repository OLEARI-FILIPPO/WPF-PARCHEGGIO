using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
//using HandyControl.Controls;

using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class ParcheggiPage : Page, INotifyPropertyChanged
    {
        ParcheggiViewModel parcheggioView;
        public event PropertyChangedEventHandler PropertyChanged;
        public string comboItemSelected;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ParcheggiPage(ParcheggiViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            parcheggioView = viewModel;
            //combo.Items.Add("Nuovo-Parcheggio");
        }

        private void RowSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            RowLabel.Content = RowSlider.Value.ToString();

            int riga = Convert.ToInt32(RowSlider.Value.ToString());
        }

        private void ColSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            ColLabel.Content = ColSlider.Value.ToString();
        }

        private void CreateDynamicRow(int riga)
        {
            for (int i = 0; i < riga; i++)
            {
                RowDefinition rd = new RowDefinition();
                // rd.Height = GridLength.Auto;
                DynamicGrid.RowDefinitions.Add(rd);
            }
        }

        private void CreateDynamicCol(int colonna)
        {
            for (int j = 0; j < colonna; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                DynamicGrid.ColumnDefinitions.Add(cd);
            }
        }

        private async void CreateDynamicGrid(string parcheggioAttuale)
        {
            int cont = 0;
            int temp = 1;

            int iRow = -1;
            foreach (RowDefinition row in DynamicGrid.RowDefinitions)
            {
                iRow++;

                int jCol = -1;
                foreach (ColumnDefinition col in DynamicGrid.ColumnDefinitions)
                {
                    jCol++;

                    TextBlock tb1 = new TextBlock()
                    {
                        Text = "\xE804",
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        FontSize = 40,
                        TextAlignment = TextAlignment.Center
                    };


                    StackPanel sp = new StackPanel()
                    {
                        Orientation = Orientation.Vertical
                    };

                    sp.Children.Add(tb1);

                    Border panel = new Border()
                    {
                        BorderThickness = new Thickness(1)
                    };


                    Grid.SetColumn(panel, jCol);
                    Grid.SetRow(panel, iRow);

                    Button b = new Button();
                    //b.Name = "btn" + cont.ToString(); ;
                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.VerticalContentAlignment = VerticalAlignment.Center;

                    b.HorizontalAlignment = HorizontalAlignment.Stretch;

                    b.VerticalAlignment = VerticalAlignment.Stretch;

                    b.Height = panel.Height;
                    b.Width = panel.Width;
                    b.Content = sp;
                    b.FontSize = 70;


                    b.Foreground = new SolidColorBrush(Colors.Black);
                    // Colore Libero : Blue
                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#93C7EA");

                    await parcheggioView.GetParkingsByName(parcheggioAttuale);
                    b.Name = "btn" + parcheggioView.ParkingObjectByName[cont].ParkingId;

                    TextBlock tb2 = new TextBlock()
                    {
                        Text = parcheggioView.ParkingObjectByName[cont].ParkingId,
                        FontSize = 18,
                        TextAlignment = TextAlignment.Center
                    };
                    sp.Children.Add(tb2);

                    // Colore Occupato : Rosso 
                    if (combo.Text != "Nuovo-Parcheggio" && parcheggioView.ParkingObjectByName != null && parcheggioView.ParkingObjectByName[cont].Stato == true)
                    {
                        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#F77B7B");

                        /*ObservableCollection<int> idveicolo = new ObservableCollection<int>();
                        idveicolo.Add(Convert.ToInt32(parcheggioView.ParkingObjectByName[cont].VehicleId));

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                            ObservableCollection<string> targhe = new ObservableCollection<string>();

                            var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/getLicence", idveicolo);
                            var data = await response.Content.ReadAsStringAsync();
                            targhe = JsonConvert.DeserializeObject<ObservableCollection<string>>(data);

                            tb2.Text += " - " + targhe[0];
                        }*/
                    }
                    b.SetResourceReference(Grid.EffectProperty, "EffectShadow2");
                    // b.Margin = new Thickness(3);
                    b.BorderThickness = new Thickness(1);
                    b.Click += clickParhceggio;
                    b.Margin = new Thickness(6);
                    panel.Child = b;

                    DynamicGrid.Children.Add(panel);
                    cont++;
                    temp++;
                }
            }
        }
      
        private async void Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            

            if (InputName.Text == "Nome Parcheggio" || RowSlider.Value == 0 || ColSlider.Value == 0 )
            {

                InputName.BorderBrush = new SolidColorBrush(Colors.Red);
                RowSlider.BorderBrush = new SolidColorBrush(Colors.Red);
                ColSlider.BorderBrush= new SolidColorBrush(Colors.Red);
                RowLabel.BorderBrush = new SolidColorBrush(Colors.Red);
                ColLabel.BorderBrush = new SolidColorBrush(Colors.Red);
                //System.Windows.MessageBox.Show("Inserire il nome del parcheggio");

                if (System.Windows.MessageBox.Show("Inserire corretamente tutti i campi", "Error", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK )
                {
                    InputName.BorderBrush = new SolidColorBrush(Colors.Gray);
                    RowLabel.BorderBrush = new SolidColorBrush(Colors.Gray);
                    ColLabel.BorderBrush = new SolidColorBrush(Colors.Gray);
                }

            }
            else
            {
                DynamicGrid.Children.Clear();
                // DynamicGrid.ShowGridLines = true;
                DynamicGrid.RowDefinitions.Clear();
                DynamicGrid.ColumnDefinitions.Clear();
                int riga = Int32.Parse(RowSlider.Value.ToString());
                int colonna = Int32.Parse(ColSlider.Value.ToString());

                await parcheggioView.CreateParcheggio(InputName.Text, riga, colonna);
                await parcheggioView.GetParkings();


            }

        }



        //Evento che visualizza i dettagli di un parcheggio
        private  void clickParhceggio(object sender, RoutedEventArgs e)
        {
            //Creo l'istanza del dettaglio e la visualizzo come window aggiuntiva

            StackPanel sp = (StackPanel)((Button)sender).Content; //prendo lo stackpanel contenuto nel parcheggio

            TextBlock tb = (TextBlock)sp.Children[1]; //prendo il textblock che contiene il nome del parcheggio

            Button b = (Button)sender;
            //apro il form che contiene i detagli nel parcheggio
            ParcheggiDetailPage parcheggiDetailPage = new ParcheggiDetailPage(tb.Text, b, combo.Text, comboItemSelected); //e passo il nome del parcheggio
            parcheggiDetailPage.ShowDialog();
            CreateDynamicGrid(combo.Text);
        }

        private void errorSlider()
        {
            ColLabel.Foreground = new SolidColorBrush(Colors.Red);
            PopupWindow popup = new PopupWindow()
            {
                MinWidth = 400,
                Title = "Title",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = true,
                AllowsTransparency = true,
                WindowStyle = WindowStyle.None
            };
            System.Windows.Controls.TextBox txtUsername = new System.Windows.Controls.TextBox();
            popup.PopupElement = new FrameworkElement();
            popup.ShowDialog();
        }

        private void ParcheggiLoaded(object sender, RoutedEventArgs e)
        {
            RowSlider.Value = 0;
            ColSlider.Value = 0;

         //   parcheggioView.GetParkings();
            RowSlider.IsEnabled = false;
            ColSlider.IsEnabled = false;
            Create.IsEnabled = false;
            InputName.IsEnabled = false;
            RowLabel.IsEnabled = false;
            ColLabel.IsEnabled = false;

            RowLabel.Content = "Row";
            ColLabel.Content = "Col";

            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = operations.ActualWidth;
            d.Duration = TimeSpan.FromSeconds(0.4);
            d.EasingFunction = new QuadraticEase();
            operations.BeginAnimation(WidthProperty, d);


            /*if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();

                    List<InfoParking> ParkingObject = JsonConvert.DeserializeObject<List<InfoParking>>(list);

                    //List<string> Parkings = new List<string>();

                    parkings.Add("Nuovo-Parcheggio");

                    foreach (var item in ParkingObject)
                    {
                        parkings.Add(item.NamePark.ToString());
                    }



                    // var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                }
            }*/
        }

        private async void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.Children.Clear();

            if(combo.SelectedItem.ToString() == "Nuovo-Parcheggio")
            {
             
                RowSlider.IsEnabled = true;
                ColSlider.IsEnabled = true;
                Create.IsEnabled = true;
                InputName.IsEnabled = true;
                RowLabel.IsEnabled = true;
                ColLabel.IsEnabled = true;
                RowSlider.Value = 0;
                ColSlider.Value = 0;

                RowLabel.Content = "Row";
                ColLabel.Content = "Col";
            }
            else
            {
                RowSlider.IsEnabled= false;
                ColSlider.IsEnabled= false;
                Create.IsEnabled = false;
                InputName.IsEnabled= false;
                RowLabel.IsEnabled = true;
                ColLabel.IsEnabled = true;

                //Mi salvo il menu selezionato per passarlo al dettaglio
                comboItemSelected = combo.SelectedItem.ToString();

                await parcheggioView.GetParkingRecords(combo.SelectedItem.ToString());
                await parcheggioView.GetRowColumn(combo.SelectedItem.ToString());




                CreaParcheggioSelezionato();

                DoubleAnimation fadeAnimation = new DoubleAnimation();
                fadeAnimation.Duration = TimeSpan.FromSeconds(1.0d);
                fadeAnimation.EasingFunction = new QuadraticEase();
                fadeAnimation.From = 0.0d;
                fadeAnimation.To = 1.0d;
                DynamicGrid.BeginAnimation(Grid.OpacityProperty, fadeAnimation);
            }
        }

        public void CreaParcheggioSelezionato()
        {
            DynamicGrid.Children.Clear();
            // DynamicGrid.ShowGridLines = true;
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();
            int riga = parcheggioView.Riga;
            int colonna = parcheggioView.Colonna;

            CreateDynamicRow(riga);

            CreateDynamicCol(colonna);

            CreateDynamicGrid(combo.Text);
        }

        private void InputNameDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InputName.Text = null;
        }

        private void InputName_GotFocus(object sender, RoutedEventArgs e)
        {
            InputName.Text = null;
        }

        private void InputName_LostFocus(object sender, RoutedEventArgs e)
        {

            if (InputName.Text == "")
            {
                InputName.Text = "Nome Parcheggio";
            }
            
        }

       
    }
}
