using System.IO;
using System.Windows;
using Fluetta.Pages.AccountPages;

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
            if (File.Exists($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}login.json") == false)
            {
                System.Diagnostics.Debug.WriteLine("[!] login.json is not found");
                ContentFrame.Navigate(typeof(LoginVariants));
            } else
            {
                System.Diagnostics.Debug.WriteLine("[!] login.json is found!");
                ContentFrame.Navigate(typeof(CurrentMethod));
            }
        }
    }
}
