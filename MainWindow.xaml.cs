using Fluetta.Pages;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;

namespace Fluetta
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            //File.WriteAllText(@".\dasd.json", JsonConvert.SerializeObject(new Instances.Instance()));
            if (File.Exists(@".\launcher_settings.json"))
            {
                Settings.SettingsData.SetFromObject(JsonConvert.DeserializeObject<Settings.SettingsData.SettingsDataObject>(File.ReadAllText(@".\launcher_settings.json")));
            } else
            {
                Settings.SettingsData.SettingsDataObject settings = Settings.SettingsData.ToObject();
                File.WriteAllText(@".\launcher_settings.json", JsonConvert.SerializeObject(settings));
                Settings.SettingsData.SetFromObject(settings);
            }
            if (!File.Exists(Settings.SettingsData.minecraftPath + "\\launcher_profiles.json"))
            {
                File.WriteAllText(Settings.SettingsData.minecraftPath + "\\launcher_profiles.json", "{\n  \"profiles\": {\n\n  }\n}");
            }
            Instances.InstanceList = Instances.ListDirs(Settings.SettingsData.minecraftPath);
            InitializeComponent();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Home));
        }

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(Settings));
            } else
            {
                ModernWpf.Controls.NavigationViewItem item = args.SelectedItem as ModernWpf.Controls.NavigationViewItem;

                switch (item.Tag.ToString())
                {
                    case "home":
                        ContentFrame.Navigate(typeof(Home));
                        break;
                    case "instances":
                        ContentFrame.Navigate(typeof(Pages.Instances));
                        break;
                }
            }
        }

        private void Account_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Account account = new Account
            {
                Owner = this
            };
            account.Show();
        }
    }
}
