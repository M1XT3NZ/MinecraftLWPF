using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using CmlLib.Core.Version;
using Newtonsoft.Json;
using Wpf.Ui.Input;

namespace MinecraftLWPF.Minecraft;

public class LMinecraft : INotifyPropertyChanged
{
    public static readonly string MinecraftPath =
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Veltrox\\.minecraft";

    private static readonly Lazy<LMinecraft> LazyInstance = new(() => new LMinecraft());

    public CMLauncher Launcher;

    public LMinecraft()
    {
        var path = new MinecraftPath(MinecraftPath);
        Launcher = new CMLauncher(path);
        Launcher.FileDownloader = new AsyncParallelDownloader(20);
        Versions = new ObservableCollection<MVersion>();
        FilteredVersions = new ObservableCollection<MVersion>();
        LocalVersions = new ObservableCollection<MVersion>();
        Filter = new VersionFilter();
        GetVersions();

        #region MCLauncher Events

        Launcher.FileChanged += e =>
        {
            Console.WriteLine("FileKind: " + e.FileKind);
            Console.WriteLine("FileName: " + e.FileName);
        };
        Launcher.ProgressChanged += (s, e) => { Console.WriteLine("{0}%", e.ProgressPercentage); };

        #endregion
    }
    public bool IsFlyoutOpen { get; set; }
    public static LMinecraft Instance => LazyInstance.Value;
    
    public ObservableCollection<MVersion> Versions { get; }
    public ObservableCollection<MVersion> LocalVersions { get; private set; }
    public ObservableCollection<MVersion> FilteredVersions { get; }
    public VersionFilter Filter { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SaveLocalVersions(string filePath = null)
    {
        if (filePath == null)
            filePath = MinecraftSettings.Instance.SettingsPath + "LocalVersions.json";

        var json = JsonConvert.SerializeObject(LocalVersions, Formatting.Indented, new TimeSpanConverter());
        File.WriteAllText(filePath, json);
    }

    public void LoadLocalVersions(string filePath = null)
    {
        if (filePath == null)
            filePath = MinecraftSettings.Instance.SettingsPath + "LocalVersions.json";

        if (!File.Exists(filePath))
        {
            LocalVersions = new ObservableCollection<MVersion>();
            return;
        }

        var json = File.ReadAllText(filePath);
        LocalVersions = JsonConvert.DeserializeObject<ObservableCollection<MVersion>>(json, new TimeSpanConverter());
        foreach (var mVersion in LocalVersions) mVersion.PlayCommand = new RelayCommand<MVersion>(StartGame!);
    }

    private void GetVersions()
    {
        // Add error handling as needed
        foreach (var version in Launcher.GetAllVersions())
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

    public async void DownloadVersion(MVersion version)
    {
        var process = await Launcher.CreateProcessAsync(version.Name, new MLaunchOption
        {
            Session = MSession.CreateOfflineSession("username")
        });
        GetVersionsLocal();
    }

    public async void GetVersionsLocal()
    {
        if (MinecraftSettings.Instance.SettingsPath + "LocalVersions.json" != null)
            await Application.Current.Dispatcher.InvokeAsync(() => { LoadLocalVersions(); });
        //LocalVersions.Clear();
        foreach (var version in await Launcher.GetAllVersionsAsync())
        {
            if (LocalVersions.FirstOrDefault(x => x.Name == version.Name) != null) return;
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (version.IsLocalVersion)
                {
                    var mvers = new MVersion
                    {
                        Name = version.Name,
                        Type = version.MType,
                        IsLocalVersion = version.IsLocalVersion,
                        PlayCommand = new RelayCommand<MVersion>(StartGame!)
                    };
                    LocalVersions.Add(mvers);
                }
            });
        }

        SaveLocalVersions();
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

    #region PlayRegion

    // Example method in your L_Minecraft class to start a game
    public async void StartGame(MVersion version)
    {
        var process = await Launcher.CreateProcessAsync(version.Name, new MLaunchOption
        {
            MaximumRamMb = MinecraftSettings.Instance.JavaMemory,
            Session = AccountsManager.Instance.CurrentSession
            
        });

        process.EnableRaisingEvents = true;
        var startTime = DateTime.Now; // Store start time
        process.Exited += (sender, args) =>
        {
            // Save playtime after the game exits
            SavePlaytime(version, startTime, DateTime.Now);
            SaveLocalVersions();
        };
        process.Start();
    }

    private void SavePlaytime(MVersion version, DateTime startTime, DateTime endTime)
    {
        var playTime = endTime - startTime;

        var localVersion = LocalVersions.FirstOrDefault(v => v.Name == version.Name);
        if (localVersion != null)
        {
            localVersion.PlayTime += playTime; // Update playtime
            OnPropertyChanged(nameof(LocalVersions)); // Notify UI to update
        }
        // Logic to save playtime, for example, to a file or settings
    }

    private void PlayVersion(object parameter)
    {
        var version = parameter as string;
        MessageBox.Show("Play version: " + version, "Play version");
        // Implement the logic to launch this version
    }

    #endregion
}

public class VersionFilter
{
    public bool Release { get; set; } = true;
    public bool Snapshot { get; set; } = false;
    public bool Beta { get; set; } = false;
    public bool Alpha { get; set; } = false;
}

public class MVersion : INotifyPropertyChanged
{
    private TimeSpan _playTime;
    public string Name { get; set; }
    public MVersionType Type { get; set; }
    public bool IsLocalVersion { get; set; }

    public TimeSpan PlayTime
    {
        get => _playTime;
        set
        {
            if (_playTime != value)
            {
                _playTime = value;
                OnPropertyChanged(nameof(PlayTime));
            }
        }
    }

    #region NotNeeded

    [JsonIgnore] public ICommand PlayCommand { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}