using ModernWpf.Media.Animation;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Fluetta.Auth;

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
