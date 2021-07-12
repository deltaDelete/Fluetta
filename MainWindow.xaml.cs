using Fluetta.Pages;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using static Fluetta.Pages.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace Fluetta
{
    public partial class MainWindow
    {
        public static List<string> versionIds;
        public static CmlLib.Core.Version.MVersionCollection versions;
        public static string latestRelease;
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
                SettingsData.SetFromObject(JsonConvert.DeserializeObject<SettingsData.SettingsDataObject>(File.ReadAllText(@".\launcher_settings.json")));
            } else
            {
                SettingsData.SettingsDataObject settings = SettingsData.ToObject();
                File.WriteAllText(@".\launcher_settings.json", JsonConvert.SerializeObject(settings));
                SettingsData.SetFromObject(settings);
            }
            if (!Directory.Exists(SettingsData.minecraftPath))
            {
                Directory.CreateDirectory(SettingsData.minecraftPath);
            }
            if (!File.Exists(SettingsData.minecraftPath + "\\launcher_profiles.json"))
            {
                File.WriteAllText(SettingsData.minecraftPath + "\\launcher_profiles.json", "{\n  \"profiles\": {\n\n  }\n}");
            }
            Instances.InstanceList = Instances.ListDirs(SettingsData.minecraftPath);
            if (File.Exists($"{SettingsData.minecraftPath}\\instances\\instance_settings.json"))
            {
                File.Delete($"{SettingsData.minecraftPath}\\instances\\instance_settings.json");
            }
            versionIds = Versions.VersionIds();
            latestRelease = Versions.LatestRelease();
            if (!Directory.Exists($"{SettingsData.minecraftPath}\\instances\\latestRelease"))
            {
                Fluetta.Instances.Instance instance = new Fluetta.Instances.Instance
                {
                    Name = "latestRelease",
                    VersionId = latestRelease,
                    Created = System.DateTime.Now,
                    LastUsed = System.DateTime.Now,
                    InstanceDir = "latestRelease",
                    JavaDir = null,
                    ResX = SettingsData.resX,
                    ResY = SettingsData.resY,
                    JVMArgs = null,
                    MaxRAM = SettingsData.maxRAM
                };
                System.Diagnostics.Debug.WriteLine("[!] Latest release chosen");
                Directory.CreateDirectory($"{SettingsData.minecraftPath}\\instances\\latestRelease");
                File.WriteAllText($"{SettingsData.minecraftPath}\\instances\\latestRelease\\instance_settings.json", JsonConvert.SerializeObject(instance));
            }
            InitializeComponent();
        }
        public class Versions
        {
            public static List<string> VersionIds() {
                List<string> versionIds = new List<string>();
                MainWindow.versions = new CmlLib.Core.CMLauncher(new CmlLib.Core.MinecraftPath(SettingsData.minecraftPath)).GetAllVersions();
                foreach (CmlLib.Core.Version.MVersionMetadata versionMetadata in versions)
                {
                    versionIds.Add(versionMetadata.Name);
                }
                return versionIds;
            }
            public static string LatestRelease()
            {
                return MainWindow.versions.LatestReleaseVersion.Name;
            }
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
