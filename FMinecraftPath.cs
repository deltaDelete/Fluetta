using System;
using System.Collections.Generic;
using System.Text;
using CmlLib.Core;

namespace Fluetta
{
    public class FMinecraftPath
    {
        public static MinecraftPath GetPath(string mainPath, string instancePath)
        {
            MinecraftPath defaultPath = new MinecraftPath(mainPath);
            MinecraftPath myPath = new MinecraftPath(instancePath)
            {
                Assets = defaultPath.Assets,
                Library = defaultPath.Library,
                Runtime = defaultPath.Runtime,
                Versions = defaultPath.Versions
            };
            return myPath;
        }
    }
}
