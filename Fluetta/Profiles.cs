﻿using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

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
            if (Directory.Exists(path + $".{Path.DirectorySeparatorChar}instances"))
            {
                string[] instances = Directory.GetDirectories($"{path}{Path.DirectorySeparatorChar}instances");
                foreach (string instance in instances)
                {
                    if (File.Exists($"{instance}{Path.DirectorySeparatorChar}instance_settings.json"))
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
                Directory.CreateDirectory($"{path}{Path.DirectorySeparatorChar}instances");
                return null;
            }
        }
        public static List<string> GetInstanceNames(List<string> list)
        {
            List<string> returnList = new List<string>();
            if (list == null)
            {
                return null;
            }
            else
            {
                foreach (string str in list)
                {
                    returnList.Add(JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{str}{Path.DirectorySeparatorChar}instance_settings.json")).Name);
                }
                return returnList;
            }
        }
        public static string GetFolderByName(string name, List<string> directories)
        {
            string path = null;
            foreach (string str in directories)
            {
                if (JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{str}{Path.DirectorySeparatorChar}instance_settings.json")).Name == name)
                {
                    path = str;
                }
            }
            return path;
        }

        public static ObservableCollection<Instance> GetInstanceObjects(string path)
        {
            ObservableCollection<Instance> objects = new ObservableCollection<Instance>();
            foreach (string instancePath in ListDirs(path))
            {
                /*
                objects.Add(new Instance()
                {
                    Name = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).Name,
                    VersionId = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).VersionId,
                    Created = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).Created,
                    LastUsed = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).LastUsed,
                    InstanceDir = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).InstanceDir,
                    JavaDir = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).JavaDir,
                    ResX = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).ResX,
                    ResY = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).ResY,
                    JVMArgs = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).JVMArgs,
                    MaxRAM = JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")).MaxRAM
                }
                );
                */
                objects.Add(JsonConvert.DeserializeObject<Instance>(File.ReadAllText($"{instancePath}{Path.DirectorySeparatorChar}instance_settings.json")));
            }
            return objects;
        }

    }
}
