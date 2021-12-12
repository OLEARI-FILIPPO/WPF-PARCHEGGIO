using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Window
    {
        public AddVehicle()
        {
            InitializeComponent();
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Targa_GotFocus(object sender, RoutedEventArgs e) //TextBox Surname
        {
            Targa.Text = null;
        }

        private void Manufacturer_GotFocus(object sender, RoutedEventArgs e)
        {
            Manufacturer.Text = null;
        }

        private void Modello_GotFocus(object sender, RoutedEventArgs e)
        {
            Model.Text = null;
        }


        private void grid_Loaded(object sender, RoutedEventArgs e)
        {

            

        }

        private void Targa_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Targa.Text == "")
            {
                Targa.Text = "Targa";
            }
        }

        private void Manufacturer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Manufacturer.Text == "")
            {
                Manufacturer.Text = "Modello";
            }
        }

        private void Modello_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Model.Text == "")
            {
                Model.Text = "Modello";
            }
        }

    }
}
