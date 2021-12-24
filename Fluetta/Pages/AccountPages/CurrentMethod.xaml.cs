using System;
using System.Collections.Generic;
using System.IO;
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
using static Fluetta.Auth;
using Newtonsoft.Json;
using ModernWpf.Media.Animation;

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для CurrentMethod.xaml
    /// </summary>
    public partial class CurrentMethod : Page
    {
        public CurrentMethod()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SessionData SessionData = JsonConvert.DeserializeObject<SessionData>(File.ReadAllText(@".\login_settings.json"));
            if (!SessionData.OnlineMode)
            {
                System.Diagnostics.Debug.WriteLine("[!] Current login method is Offline");
                LoginMethod.Text = Properties.Resources.Offline;
                Username.Text = SessionData.Session.Username;
            } else if (SessionData.OnlineMode)
            {
                System.Diagnostics.Debug.WriteLine("[!] Current login method is Mojang");
                LoginMethod.Text = Properties.Resources.Mojang;
                Username.Text = SessionData.Session.Username;
            }
        }

        private void ChangeLoginMethod(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo _transitionInfo = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(LoginVariants), null, _transitionInfo);
        }
    }
}
