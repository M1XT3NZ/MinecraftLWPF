

using System;
using System.Windows;
using System.Windows.Controls;
using MinecraftLWPF.Minecraft;

namespace MinecraftLWPF.pages;

public partial class Settings : Page
{
    private MinecraftSettings _minecraftSettings;
    public Settings()
    {
        InitializeComponent();
        LoadSettings();
        this.Loaded += OnPageLoaded;
    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _minecraftSettings.Save();
    }
    private void LoadSettings()
    {
        // Load the settings from the file or create a new instance with defaults
        _minecraftSettings = MinecraftSettings.Load();

        // Set the DataContext for the page
        this.DataContext = _minecraftSettings;
    }
    private void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        AddMemoryMilestoneLabels();
        MemorySlider.SizeChanged += (s, args) => PositionMemoryLabels();
    }

    private void AddMemoryMilestoneLabels()
    {
        var memoryMilestones = ((MinecraftSettings)DataContext).MemoryMilestones;
        foreach (var milestone in memoryMilestones)
        {
            var label = new TextBlock
            {
                Text = $"{milestone / 1024} GB"
            };
            MemoryMilestoneCanvas.Children.Add(label);
            label.Loaded += (s, args) => PositionMemoryLabels(); // Position labels once they're loaded
        }
    }

    private void PositionMemoryLabels()
    {
        var memoryMilestones = ((MinecraftSettings)DataContext).MemoryMilestones;
        double maxMemory = ((MinecraftSettings)DataContext).MaxMemory;

        for (int i = 0; i < MemoryMilestoneCanvas.Children.Count; i++)
        {
            var label = (TextBlock)MemoryMilestoneCanvas.Children[i];
            int memoryValue = memoryMilestones[i];
            SetLabelPosition(label, memoryValue, maxMemory);
        }
    }

    private void SetLabelPosition(TextBlock label, int memoryValue, double maxMemory)
    {
        if (MemorySlider.ActualWidth > 0 && label.ActualWidth > 0)
        {
            // Calculate the position as a ratio of the memory value to the max memory
            double positionRatio = memoryValue / maxMemory;
            // Calculate the actual position by considering the label width to center-align the label
            double position = (MemorySlider.ActualWidth - label.ActualWidth) * positionRatio;
            // Ensure the label does not go beyond the left edge of the slider
            position = Math.Max(position, 0);
            // Ensure the label does not go beyond the right edge of the slider
            position = Math.Min(position, MemorySlider.ActualWidth - label.ActualWidth);

            Canvas.SetLeft(label, position);
        }
    }

}