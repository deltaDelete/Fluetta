using ModernWpf.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для OfflineLogin.xaml
    /// </summary>
    public partial class OfflineLogin
    {
        public OfflineLogin()
        {
            InitializeComponent();
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo _transitionInfo = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(LoginVariants), null, _transitionInfo);
        }
        private void LoginTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoginHint.Foreground = LoginTextBox.IsKeyboardFocused || LoginTextBox.Text.Length > 0 ? Brushes.Transparent : Brushes.Black;
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Auth.Authentificate(LoginTextBox.Text, false);
            System.Diagnostics.Debug.WriteLine("[!] Offline login succussful");
            SlideNavigationTransitionInfo _transitionInfo = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(CurrentMethod), null, _transitionInfo);
        }
    }
}
