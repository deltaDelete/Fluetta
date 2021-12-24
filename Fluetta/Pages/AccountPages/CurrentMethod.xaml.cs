using System.IO;
using System.Windows;
using System.Windows.Controls;
using static Fluetta.Auth;
using Newtonsoft.Json;
using ModernWpf.Media.Animation;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для CurrentMethod.xaml
    /// </summary>
    public partial class CurrentMethod : Page
    {
        private SessionData SessionData;
        private readonly SlideNavigationTransitionInfo _transitionInfo = new() { Effect = SlideNavigationTransitionEffect.FromRight };
        public CurrentMethod()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SessionData = JsonConvert.DeserializeObject<SessionData>(File.ReadAllText(@$".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}login.json"));
            if (SessionData.OnlineMode && !SessionData.IsMicrosoft)
            {
                System.Diagnostics.Debug.WriteLine("[!] Current login method is Mojang");
                LoginMethod.Text = Properties.Resources.Mojang;
                Username.Text = SessionData.Session.Username;
            }
            else if (SessionData.OnlineMode && SessionData.IsMicrosoft)
            {
                System.Diagnostics.Debug.WriteLine("[!] Current login method is Microsoft");
                LoginMethod.Text = Properties.Resources.MicrosoftLogin;
                Username.Text = SessionData.Session.Username;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[!] Current login method is Offline");
                LoginMethod.Text = Properties.Resources.Offline;
                Username.Text = SessionData.Session.Username;
            }
        }

        private void ChangeLoginMethod(object sender, RoutedEventArgs e)
        {
            if (SessionData.IsMicrosoft && SessionData.OnlineMode)
            {
                MicrosoftLoginWindow loginForm = new();
                loginForm.ShowLogoutDialog();
            }
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(LoginVariants), null, _transitionInfo);
        }
    }
}
