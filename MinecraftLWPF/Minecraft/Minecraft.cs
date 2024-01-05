using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CmlLib.Core;
using CmlLib.Core.Version;

namespace MinecraftLWPF.Minecraft
{
    public class L_Minecraft : INotifyPropertyChanged
    {
        public static readonly string MinecraftPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Veltrox\\.minecraft";

        private static readonly Lazy<L_Minecraft> lazyInstance = new Lazy<L_Minecraft>(() => new L_Minecraft());
        public static L_Minecraft Instance => lazyInstance.Value;

        private CMLauncher launcher;

        public ObservableCollection<MVersion> Versions { get; }
        public ObservableCollection<MVersion> FilteredVersions { get; }
        public VersionFilter Filter { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public L_Minecraft()
        {
            var path = new MinecraftPath(MinecraftPath);
            launcher = new CMLauncher(path);
            Versions = new ObservableCollection<MVersion>();
            FilteredVersions = new ObservableCollection<MVersion>();
            Filter = new VersionFilter();
            GetVersions();
        }

        private void GetVersions()
        {
            // Add error handling as needed
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

        public async Task FilterVersions()
        {
            FilteredVersions.Clear();

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (Filter is { Release: false, Snapshot: false, Beta: false, Alpha: false })
                    // No checkboxes are checked, add all versions
                    foreach (var version in Versions)
                        FilteredVersions.Add(version);
                else
                    foreach (var version in Versions)
                    {
                        var isFilteredVersion = (version.Type == MVersionType.Release && Filter.Release) ||
                                                (version.Type == MVersionType.Snapshot && Filter.Snapshot) ||
                                                (version.Type == MVersionType.OldBeta && Filter.Beta) ||
                                                (version.Type == MVersionType.OldAlpha && Filter.Alpha);

                        if (isFilteredVersion) FilteredVersions.Add(version);
                    }
            });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class VersionFilter
    {
        public bool Release { get; set; } = true;
        public bool Snapshot { get; set; } = false;
        public bool Beta { get; set; } = false;
        public bool Alpha { get; set; } = false;
    }

    public class MVersion
    {
        public string Name { get; set; }
        public MVersionType Type { get; set; }
        public bool IsLocalVersion { get; set; }
    }
}
