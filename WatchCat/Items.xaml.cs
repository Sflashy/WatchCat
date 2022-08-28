using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WatchCat.Models;

namespace WatchCat
{
    public partial class Items : Page
    {
        private readonly List<string> _relics = new List<string> { "Axi", "Lith", "Meso", "Neo" };
        private dynamic _filteredItems;
        private List<Item> _itemList;

        public Items()
        {
            InitializeComponent();
        }
        private void OnSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_itemList == null) return;
            var foundItems = new List<Item>();
            
            foreach (var item in _itemList)
            {
                // filter prime parts by name
                if (!string.IsNullOrEmpty(item.Name) && item.Name.ToLower().Contains(SearchBar.Text.ToLower()))
                {
                    foundItems.Add(item);
                }
                // filter prime parts by relic
                item.Relics.ForEach(relic =>
                {
                    if (relic.ToLower().Contains(SearchBar.Text.ToLower()))
                    {
                        if (foundItems.Contains(item)) return;
                        foundItems.Add(item);
                    }
                });
            }
            
            if (!string.IsNullOrEmpty(SearchBar.Text))
            {
                DataGrid.ItemsSource = foundItems;
            }
            else
            {
                DataGrid.ItemsSource = _itemList;
            }
        }
        private async void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await UpdateItemData();

            if (_itemList == null) return;

            _relics.ForEach(r => UpdateRelicData(r));

            DataGrid.ItemsSource = _itemList;

            while (true)
            {
                await UpdatePrices();
                await Task.Delay(new TimeSpan(0, 5, 0));
            }
        }
        private async Task UpdateItemData()
        {
            _filteredItems = await AppManager.HttpRequest("https://api.warframestat.us/wfinfo/filtered_items");
            if (_filteredItems == null) return;
            _itemList = new List<Item>();
            foreach (dynamic item in _filteredItems.eqmt)
            {
                foreach (dynamic part in item.Value.parts)
                {
                    string ducats = part.Value.ducats;
                    if (ducats == null) continue;
                    string name = part.Name;
                    bool vaulted = (part.Value.vaulted == "true") ? true : false;
                    string wfmLink = name.Replace(' ', '_').ToLower(); 
                    _itemList.Add(new Item
                    {
                        Name = name,
                        Vaulted = vaulted,
                        Ducats = int.Parse(ducats),
                        WFMUrl = "https://warframe.market/items/" + wfmLink
                    });
                }
            }
        }
        private async Task UpdatePrices()
        {
            dynamic prices = await AppManager.HttpRequest("https://api.warframestat.us/wfinfo/prices");
            if (prices == null) return;
            foreach (dynamic item in prices)
            {
                string itemName = item.name;
                if (Regex.Match(itemName, @"Chassis|System|Neuroptics|Wings").Success) itemName = itemName.Replace(" Blueprint", "");
                Item _item = _itemList.FirstOrDefault(x => x.Name == itemName);
                if (_item == null) continue;
                _item.Average = item.custom_avg;
                _item.Volume = item.today_vol;
            }
        }
        private void UpdateRelicData(string relic)
        {
            if (_filteredItems == null) return;
            foreach (dynamic relicName in _filteredItems.relics[relic])
            {
                foreach (var item in relicName.First)
                {
                    string itemName = item.Value;
                    Item _item = _itemList.FirstOrDefault(x => x.Name == itemName);
                    if (_item == null) continue;
                    _item.Relics.Add(relic + " " + relicName.Name);
                }
            }
        }
        private void OnRowDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is DataGridCell)
                {
                    var grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        var selectedItem = grid.SelectedItem as Item;
                        if (selectedItem != null)
                        {
                            OpenInBrowser(selectedItem.WFMUrl);
                        }
                    }
                }
            }
        }
        private void OpenInBrowser(string url)
        {
            Process.Start(url);
        }
    }
}
