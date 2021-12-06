using HandyControl.Tools;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per CreateOwner.xaml
    /// </summary>
    public partial class CreateOwner : Window
    {
        public CreateOwner()
        {
            InitializeComponent();
            ConfigHelper.Instance.SetLang("it");

            this.DataContext = this;
        }
    }
}
