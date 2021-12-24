using Fluetta.Pages;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using static Fluetta.Pages.Settings;
using System.Collections.ObjectModel;

namespace Fluetta
{
    public partial class MainWindow
    {
        public static ObservableCollection<string> versionIds;
        public static CmlLib.Core.Version.MVersionCollection versions;
        public static string latestRelease;
        public MainWindow()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            Directory.CreateDirectory($".{Path.DirectorySeparatorChar}settings");
            if (File.Exists($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json"))
            {
                SettingsData.SetFromObject(JsonConvert.DeserializeObject<SettingsData.SettingsDataObject>(File.ReadAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json")));
            } else
            {
                SettingsData.SettingsDataObject settings = SettingsData.ToObject();
                File.WriteAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json", JsonConvert.SerializeObject(settings));
                SettingsData.SetFromObject(settings);
            }
            if (!Directory.Exists(SettingsData.minecraftPath))
            {
                Directory.CreateDirectory(SettingsData.minecraftPath);
            }
            if (!File.Exists(SettingsData.minecraftPath + Path.DirectorySeparatorChar + "launcher_profiles.json"))
            {
                File.WriteAllText(SettingsData.minecraftPath + Path.DirectorySeparatorChar + "launcher_profiles.json", "{\n  \"profiles\": {\n\n  }\n}");
            }
            Instances.InstanceList = Instances.ListDirs(SettingsData.minecraftPath);
            if (File.Exists($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}instance_settings.json"))
            {
                File.Delete($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}instance_settings.json");
            }
            versionIds = Versions.VersionIds();
            latestRelease = Versions.LatestRelease();
            if (!Directory.Exists($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}latestRelease"))
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
                Directory.CreateDirectory($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}latestRelease");
                File.WriteAllText($"{SettingsData.minecraftPath}{Path.DirectorySeparatorChar}instances{Path.DirectorySeparatorChar}latestRelease{Path.DirectorySeparatorChar}instance_settings.json", JsonConvert.SerializeObject(instance));
            }
            InitializeComponent();

        }
        public class Versions
        {
            public static ObservableCollection<string> VersionIds() {
                ObservableCollection<string> versionIds = new ObservableCollection<string>();
                try
                {
                    MainWindow.versions = new CmlLib.Core.CMLauncher(new CmlLib.Core.MinecraftPath(SettingsData.minecraftPath)).GetAllVersions();
                }
                catch
                {
                    MessageBox.Show("No internet connection available, launcher won't work");
                    MainWindow.versions = new CmlLib.Core.VersionLoader.LocalVersionLoader(new CmlLib.Core.MinecraftPath(SettingsData.minecraftPath)).GetVersionMetadatas();
                }
                foreach (CmlLib.Core.Version.MVersionMetadata versionMetadata in versions)
                {
                    versionIds.Add(versionMetadata.Name);
                }
                return versionIds;
            }
            public static string LatestRelease()
            {
                try
                {
                    return MainWindow.versions.LatestReleaseVersion.Name;
                }
                catch
                {
                    return "";
                }
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
