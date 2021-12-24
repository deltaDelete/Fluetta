using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CmlLib.Core.Auth;
using Fluetta;
using Fluetta.Pages.AccountPages;
using ModernWpf.Media.Animation;
using Newtonsoft.Json;
using static Fluetta.Auth;

namespace Fluetta
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account
    {
        public Account()
        {
            InitializeComponent();
        }
        //protected override void OnSourceInitialized(EventArgs e) => IconHelper.RemoveIcon(this);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ContentFrame.Navigate(typeof(CurrentMethod));
            if (File.Exists(@".\login_settings.json") == false)
            {
                System.Diagnostics.Debug.WriteLine("[!] login_settings.json is not found");
                ContentFrame.Navigate(typeof(LoginVariants));
            } else
            {
                System.Diagnostics.Debug.WriteLine("[!] login_settings.json is found!");
                ContentFrame.Navigate(typeof(CurrentMethod));
            }
        }
    }
}
