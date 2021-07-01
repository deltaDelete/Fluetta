using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Fluetta;

namespace Fluetta
{
    public class Instances
    {
        public class Instance
        {
            public string Name { get; set; }
            public string VersionId { get; set; }
            public DateTime Created { get; set; }
            public DateTime LastUsed { get; set; }
            public string InstanceDir { get; set; }
            public string JavaDir { get; set; }
            public string ResX { get; set; }
            public string ResY { get; set; }
            public string JVMArgs { get; set; }
            public string MaxRAM { get; set; }
        }
        public static List<string> InstanceList = new List<string>();
        public static List<string> ListDirs(string path)
        {
            List<string> instanceList = new List<string>();
            if (Directory.Exists(path + ".\\instances"))
            {
                string[] instances = Directory.GetDirectories($"{path}\\instances");
                foreach (string instance in instances)
                {
                    if (File.Exists($"{instance}\\instance_settings.json"))
                    {
                        System.Diagnostics.Debug.WriteLine($"[!] Folder {instance} added to List");
                        instanceList.Add(instance);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[!] Folder {instance} doesn\'t contain instance_settings.json");
                    }
                }
                return instanceList;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[!] Folder instances not found");
                return null;
            }
        }
        public static List<string> GetInstanceNames(List<string> list)
        {
            List<string> returnList = new List<string>();
            foreach (string str in list)
            {
                returnList.Add(JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{str}\\instance_settings.json")).Name);
            }
            return returnList;
        }
        public static string GetFolderByName(string name, List<string> directories)
        {
            string path = null;
            foreach (string str in directories)
            {
                if (JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{str}\\instance_settings.json")).Name == name)
                {
                    path = str;
                }
            }
            return path;
        }
    }
}
