using System.IO;
using Newtonsoft.Json;

namespace Fluetta
{
    public static class Settings
    {
        public static string Accent { get; set; } = "#FF0078D7";
        public static string Theme { get; set; } = "systemdefault";
        public static string Language { get; set; } = System.Globalization.CultureInfo.CurrentUICulture.Name;
        public static string MinecraftPath { get; set; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + $"{Path.DirectorySeparatorChar}.fluetta";
        public static string ResX { get; set; } = "856";
        public static string ResY { get; set; } = "482";
        public static string JavaPath { get; set; } = "default";
        public static string JVMArgs { get; set; } = "";
        public static string MaxRAM { get; set; } = "2048";
        public static bool CloseAfterStart { get; set; } = true;
        static Settings()
        {
            _Settings settings = new();
            Directory.CreateDirectory($".{Path.DirectorySeparatorChar}settings");
            if (File.Exists($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json"))
            {
                settings = JsonConvert.DeserializeObject<_Settings>(File.ReadAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json"));
                Accent = settings.Accent;
                Theme = settings.Theme;
                Language = settings.Language;
                MinecraftPath = settings.MinecraftPath;
                ResX = settings.ResX;
                ResY = settings.ResY;
                JavaPath = settings.JavaPath;
                JVMArgs = settings.JVMArgs;
                MaxRAM = settings.MaxRAM;
                CloseAfterStart = settings.CloseAfterStart;
            }
            else
            {
                File.WriteAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json", JsonConvert.SerializeObject(settings));
            }
        }
        private class _Settings
        {
            public string Theme { get; set; } = Settings.Theme;
            public string Accent { get; set; } = Settings.Accent;
            public string Language { get; set; } = Settings.Language;
            public string MinecraftPath { get; set; } = Settings.MinecraftPath;
            public string ResX { get; set; } = Settings.ResX;
            public string ResY { get; set; } = Settings.ResY;
            public string JavaPath { get; set; } = Settings.JavaPath;
            public string JVMArgs { get; set; } = Settings.JVMArgs;
            public string MaxRAM { get; set; } = Settings.MaxRAM;
            public bool CloseAfterStart { get; set; } = Settings.CloseAfterStart;
        }
        public static void Save() => File.WriteAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}launcher_settings.json", JsonConvert.SerializeObject(new _Settings()));
    }
}
