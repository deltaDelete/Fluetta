using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CmlLib.Core.Auth;
using ModernWpf.Media.Animation;

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для MojangLogin.xaml
    /// </summary>
    public partial class MojangLogin : Page
    {
        public static MLoginResponse response;
        private readonly SlideNavigationTransitionInfo _transitionInfo = new() { Effect = SlideNavigationTransitionEffect.FromRight };

        public MojangLogin()
        {
            InitializeComponent();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(LoginVariants), null, _transitionInfo);
        }
        public void LoginClick(object sender, RoutedEventArgs e)
        {
            if (Auth.IsSuccess(LoginTextBox.Text, PasswordTextBox.Password))
            {
                ErrorBox.Visibility = Visibility.Collapsed;
                Auth.Authentificate(LoginTextBox.Text, true, PasswordTextBox.Password);
                System.Diagnostics.Debug.WriteLine("[!] Mojang login successful");
                ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(CurrentMethod), null, _transitionInfo);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[!] Mojang login failed");
                ErrorBox.Visibility = Visibility.Visible;
            }
        }

        private void LoginTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoginHint.Foreground = LoginTextBox.IsKeyboardFocused || LoginTextBox.Text.Length > 0 ? Brushes.Transparent : Brushes.Black;
        }
        private void PasswordTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordHint.Foreground = PasswordTextBox.IsKeyboardFocused || PasswordTextBox.Password.Length > 0 ? Brushes.Transparent : Brushes.Black;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorBox.Visibility = Visibility.Collapsed;
        }
    }
}
