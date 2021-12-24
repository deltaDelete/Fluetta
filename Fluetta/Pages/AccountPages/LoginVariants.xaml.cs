using ModernWpf.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для LoginVariants.xaml
    /// </summary>
    public partial class LoginVariants : Page
    {
        private readonly SlideNavigationTransitionInfo _transitionInfo = new() { Effect = SlideNavigationTransitionEffect.FromRight };
        public LoginVariants()
        {
            InitializeComponent();
        }

        private void MojangLogin_Click(object sender, RoutedEventArgs e)
        {
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(MojangLogin), null, _transitionInfo);
        }

        private void OfflineLogin_Click(object sender, RoutedEventArgs e)
        {
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(OfflineLogin), null, _transitionInfo);
        }
        private void MicrosoftLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Auth.Authentificate(microsoft: true) != null)
                ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(CurrentMethod), null, _transitionInfo);
        }
    }
}
