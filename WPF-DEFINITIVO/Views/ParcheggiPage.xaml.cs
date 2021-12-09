using HandyControl.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private void Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            DynamicGrid.Children.Clear();
           // DynamicGrid.ShowGridLines = true;
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();
            int riga = Int32.Parse(RowSlider.Value.ToString());
            int colonna = Int32.Parse(ColSlider.Value.ToString());

            //int altezzaIniziale = Int32.Parse(StaticGrid.RowDefinitions[0].Height.ToString()) + Int32.Parse(StaticGrid.RowDefinitions[1].Height.ToString());
            //DynamicGrid.Width = PageParcheggio.Width - 5;

            //DynamicGrid.Height = PageParcheggio.ActualHeight - altezzaIniziale;
            //double widthButton = DynamicGrid.Width * 0.9 / colonna;
            //double heightButton = DynamicGrid.Height * 0.87 / riga;
            /*double widthButton = (DynamicGrid.Width - (nRiduzioneColonna * colonna)) / colonna;
            double heightButton = (DynamicGrid.Height - (nRiduzioneRiga * riga)) / riga;*/
            /*if (colonna == 0)
            if(riga == 0)*/

            

            for (int i = 0; i < riga; i++)
            {
                RowDefinition rd = new RowDefinition();
               // rd.Height = GridLength.Auto;
                DynamicGrid.RowDefinitions.Add(rd);
               

            }

            for (int j = 0; j < colonna; j++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                //   cd.Width = GridLength.Auto;
                DynamicGrid.ColumnDefinitions.Add(cd);
            }

            int cont = 0;
            int temp = 1;
            //for (int i = 0; i < riga; i++)
            //{
            //    for (int j = 0; j < colonna; j++)
            //    {
            //        TextBlock tb1 = new TextBlock()
            //        {
            //            Text = "\xE804",
            //            FontFamily = new FontFamily("Segoe MDL2 Assets"),
            //            FontSize = 50,
            //            TextAlignment = TextAlignment.Center
            //        };

            //        TextBlock tb2 = new TextBlock()
            //        {
            //            Text = "P0" + temp,
            //            FontSize = 18,
            //            TextAlignment = TextAlignment.Center

            //        };

            //        StackPanel sp = new StackPanel()
            //        {
            //            Orientation = Orientation.Vertical
            //        };

            //        sp.Children.Add(tb1);
            //        sp.Children.Add(tb2);

            //        Border panel = new Border()
            //        {
            //            BorderThickness = new Thickness(1)
            //        };

            //        Button b = new Button();
            //        b.Name = "btn" + cont.ToString();
            //        //  b.Width = widthButton;
            //        //  b.Height = heightButton;
            //        b.Content = sp;
            //        b.FontSize = 70;
            //        b.Foreground = new SolidColorBrush(Colors.Black);
            //        // Colore Libero : Verde 
            //        b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#8BE78B");
            //        // Colore Occupato : Rosso 
            //        //b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#F77B7B");
            //        b.SetResourceReference(Grid.EffectProperty, "EffectShadow2");
            //        b.Margin = new Thickness(3);
            //        //b.BorderThickness = new Thickness(1);
            //        b.Click += clickParhceggio;
            //        panel.Child = b;
            //        Grid.SetColumn(panel, j);
            //        Grid.SetRow(panel, i);

            //        DynamicGrid.Children.Add(panel);

            //        cont++;
            //        temp++;
            //    }
            //}



            int iRow = -1;
            foreach ( RowDefinition row in DynamicGrid.RowDefinitions)
            {

                iRow++;

                int jCol = -1;
                foreach (ColumnDefinition item in DynamicGrid.ColumnDefinitions)
                {

                    jCol++;

                    TextBlock tb1 = new TextBlock()
                    {
                        Text = "\xE804",
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        FontSize = 40,
                        TextAlignment = TextAlignment.Center
                    };

                    TextBlock tb2 = new TextBlock()
                    {

                       // string text = "P0" + iRow.ToString() + jCol.ToString(),
                        Text = "P0" + temp.ToString(),
                        FontSize = 18,
                        TextAlignment = TextAlignment.Center

                    };

                    StackPanel sp = new StackPanel()
                    {
                        Orientation = Orientation.Vertical
                    };

                    sp.Children.Add(tb1);
                    sp.Children.Add(tb2);

                    Border panel = new Border()
                    {
                        BorderThickness = new Thickness(1)
                    };

                   // panel.BorderBrush = new SolidColorBrush(Colors.Black);

                    Grid.SetColumn(panel, jCol);
                    Grid.SetRow(panel, iRow);

                    Button b = new Button();
                    b.Name = "btn" + cont.ToString(); ;
                    //  b.Width = widthButton;
                    //  b.Height = heightButton;

                    b.HorizontalContentAlignment = HorizontalAlignment.Center;
                    b.VerticalContentAlignment = VerticalAlignment.Center;

                    b.HorizontalAlignment = HorizontalAlignment.Stretch;

                    b.VerticalAlignment = VerticalAlignment.Stretch;

                    b.Height = panel.Height-20;
                    b.Width = panel.Width-20;
                    b.Content = sp;
                    b.FontSize = 70;

                    b.Foreground = new SolidColorBrush(Colors.Black);
                    // Colore Libero : Verde 
                    b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#93C7EA");
                    // Colore Occupato : Rosso 
                    //b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#F77B7B");
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

        //Evento che visualizza i dettagli di un parcheggio
        private void clickParhceggio(object sender, RoutedEventArgs e)
        {
            //Creo l'istanza del dettaglio e la visualizzo come window aggiuntiva

            StackPanel sp = (StackPanel)((Button)sender).Content; //prendo lo stackpanel contenuto nel parcheggio

            TextBlock tb = (TextBlock)sp.Children[1]; //prendo il textblock che contiene il nome del parcheggio

            //apro il form che contiene i detagli nel parcheggio
            ParcheggiDetailPage parcheggiDetailPage = new ParcheggiDetailPage(tb.Text); //e passo il nome del parcheggio
            parcheggiDetailPage.ShowDialog();
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

    

        private async void ParcheggiLoaded(object sender, RoutedEventArgs e)
        {
            parcheggioView.GetParkings();
            

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

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.Children.Clear();

            int riga = parcheggioView.ReturnRowCol(combo.SelectedItem.ToString())[0];
            int colonna = parcheggioView.ReturnRowCol(combo.SelectedItem.ToString())[1];
        }
    }
}
