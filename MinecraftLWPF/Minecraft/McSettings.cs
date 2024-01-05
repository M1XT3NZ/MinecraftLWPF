using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management; // Make sure to add a reference to System.Management

namespace MinecraftLWPF.Minecraft
{
    public class MinecraftSettings
    {
        [JsonIgnore]
        public int[] MemoryMilestones { get; set; } = { 1024, 8192, 16384, 32768 };
        [JsonIgnore]
        public string SettingsPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Veltrox\\";
        public int JavaMemory { get; set; } = 2048;

        [JsonIgnore]
        public List<int> JavaMemoryOptions { get; private set; }

        public string JavaCustomArguments { get; set; } = "";

        [JsonIgnore]
        public long MaxMemory => GetTotalPhysicalMemory() / 1024 / 1024; // MB

        public string Language { get; set; } = "en";
        public bool EnableLogging { get; set; } = true;

        public MinecraftSettings()
        {
            JavaMemoryOptions = GetMemoryOptions();
        }

        private List<int> GetMemoryOptions()
        {
            long totalMemory = GetTotalPhysicalMemory();
            var options = new List<int>();

            for (int i = 1024; i <= totalMemory / 1024 / 1024; i += 1024)
            {
                options.Add(i);
            }

            return options;
        }

        private long GetTotalPhysicalMemory()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
            {
                foreach (var item in searcher.Get())
                {
                    return Convert.ToInt64(item.Properties["TotalPhysicalMemory"].Value);
                }
            }
            return 0;
        }

        public void Save(string filePath = null)
        {
            if(filePath == null)
                filePath = SettingsPath + "Settings.json";
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static MinecraftSettings Load(string filePath = null)
        {
            if(filePath == null)
                filePath = new MinecraftSettings().SettingsPath + "Settings.json";
            if (!File.Exists(filePath))
                return new MinecraftSettings();

            return JsonConvert.DeserializeObject<MinecraftSettings>(File.ReadAllText(filePath));
        }
    }
}
