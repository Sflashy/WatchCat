using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using WatchCat.Core.Common;
using WatchCat.Core.Extensions;
using WatchCat.Core.Models;
using WatchCat.Core.Services;

namespace WatchCat.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isUpdating;

    [ObservableProperty]
    private string query;

    [ObservableProperty]
    private ObservableCollection<Part> filteredParts;

    [ObservableProperty]
    private ObservableCollection<Part> parts;

    public ISnackbarMessageQueue MessageQueue { get; }

    private readonly HttpClientService _httpClientService;
    public MainViewModel(HttpClientService httpClientService, ISnackbarMessageQueue messageQueue)
    {
        _httpClientService = httpClientService;
        MessageQueue = messageQueue;
        CheckForUpdates();
        FetchIAndUpdatePartPricesAsync();
    }

    [RelayCommand]
    private void OpenInBrowser(object item)
    {
        if(item is Part part)
        {
            string partName = part.Name.Replace(" ", "_").ToLower();
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://warframe.market/items/" + partName,
                UseShellExecute = true
            });
        }
        
    }
    private async void FetchIAndUpdatePartPricesAsync()
    {
        IsUpdating = true;
        IResult<Data> result = await _httpClientService.GetFromJsonAsync<Data>("https://api.warframestat.us/wfinfo/filtered_items/");
        if (!result.IsSuccess)
        {
            Log.Error(result.Message);
            return;
        }
        UpdatePartPricesAsync(result.Data.Parts);

    }

    private async void UpdatePartPricesAsync(List<Part> parts)
    {
        IResult<dynamic> result = await _httpClientService.GetFromJsonAsync<dynamic>("https://api.warframestat.us/wfinfo/prices");
        if (!result.IsSuccess)
        {
            Log.Error(result.Message);
            return;
        }
        foreach (dynamic item in result.Data)
        {
            string name = item.name;
            if (Regex.Match(name, @"Chassis|System|Neuroptics|Wings").Success) name = name.Replace(" Blueprint", "");
            Part part = parts.FirstOrDefault(x => x.Name == name);
            if (part == null) continue;
            part.Average = item.custom_avg;
            part.Volume = item.today_vol;
        }

        Parts = parts.ToObservableCollection();
        FilteredParts = Parts;
        IsUpdating = false;
    }

    private async void CheckForUpdates()
    {
        IResult<string> result = await _httpClientService.GetStringAsync("https://raw.githubusercontent.com/Sflashy/WatchCat/refs/heads/master/WatchCat/version.txt");
        if (!result.IsSuccess)
        {
            MessageQueue.Enqueue(result.Message);
            return;
        }
        string latestVersion = result.Data;
        string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        if (currentVersion != latestVersion)
            MessageQueue.Enqueue($"A new version of WatchCat is available. Please update to version {latestVersion}");
    }

    #region RelayCommands
    [RelayCommand]
    private void Filter()
    {
        FilteredParts = Parts
            .Where(part => part.Name.Contains(Query, StringComparison.CurrentCultureIgnoreCase) || part.Relic.Contains(Query, StringComparison.CurrentCultureIgnoreCase))
            .ToObservableCollection();
    }

    [RelayCommand]
    private void DragWindow(Window window)
    {
        try
        {
            window.DragMove();
        }
        catch (Exception)
        {
            //
        }
    }

    [RelayCommand]
    private void MinimizeWindow(Window window)
    {
        window.WindowState = WindowState.Minimized;
    }
    [RelayCommand]
    private void SwitchWindowState(Window window)
    {
        window.WindowState = window.WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    [RelayCommand]
    private void CloseWindow(Window window)
    {
        Application.Current.Shutdown();
    }

    #endregion

}
