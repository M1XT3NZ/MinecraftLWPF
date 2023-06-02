using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.VersionMetadata;


namespace MinecraftLWPF.Minecraft;

public class Minecraft
{
    public static readonly string MinecraftPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "Veltrox\\.minecraft";
     private static CMLauncher launcher { get; set; }

    public static ObservableCollection<MVersion>? Versions { get; set; }
    
    public Minecraft()
    {
        MinecraftPath path = new MinecraftPath(MinecraftPath);
        launcher = new CMLauncher(path);
        Versions = new ObservableCollection<MVersion>();
        GetVersions();
    }
    
    private void GetVersions()
    {
        
        foreach (var version in launcher.GetAllVersions())
        {
            var mvers = new MVersion
            {
                Name = version.Name,
                Type = version.MType,
                IsLocalVersion = version.IsLocalVersion
            };
            Versions.Add(mvers);
        }
    }
    
}

public class MVersion
{
    public string Name { get; set; }
    public MVersionType Type { get; set; }
    public bool IsLocalVersion { get; set; }
}