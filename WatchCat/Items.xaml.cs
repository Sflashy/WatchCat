using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WatchCat
{
    public partial class Items : Page
    {
        private List<Item> _items;

        public Items()
        {
            InitializeComponent();
        }

        private void ItemSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_items == null) return;
            var foundItems = new List<Item>();
            foreach (var item in _items)
            {
                if (!string.IsNullOrEmpty(item.Name) && item.Name.ToLower().Contains(ItemSearch.Text.ToLower()))
                {
                    foundItems.Add(item);
                }
            }
            if (!string.IsNullOrEmpty(ItemSearch.Text))
            {
                DataGrid.ItemsSource = foundItems;
            }
            else
            {
                DataGrid.ItemsSource = _items;
            }
        }

        private async void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            while (true)
            {
                await UpdateItems();
                await UpdatePrices();
                await Task.Delay(new TimeSpan(0, 5, 0));
            }
        }

        private async Task UpdateItems()
        {
            _items = new List<Item>();
            dynamic items = await AppManager.HttpRequest("https://api.warframestat.us/wfinfo/filtered_items");

            foreach (dynamic item in items.eqmt)
            {
                foreach (dynamic part in item.Value.parts)
                {
                    string ducats = part.Value.ducats;
                    if (ducats == null) continue;
                    string name = part.Name;
                    bool vaulted = (part.Value.vaulted == "true") ? true : false;

                    _items.Add(new Item
                    {
                        Name = name,
                        Vaulted = vaulted,
                        Ducats = int.Parse(ducats)
                    });
                }
            }
        }

        private async Task UpdatePrices()
        {
            dynamic prices = await AppManager.HttpRequest("https://api.warframestat.us/wfinfo/prices");

            foreach (dynamic item in prices)
            {
                string itemName = item.name;
                if (Regex.Match(itemName, @"Chassis|System|Neuroptics|Wings").Success) itemName = itemName.Replace(" Blueprint", "");
                Item _item = _items.FirstOrDefault<Item>(x => x.Name == itemName);
                if (_item == null) continue;
                _item.Avarage = item.custom_avg;
                _item.Volume = item.today_vol;
            }

            DataGrid.ItemsSource = _items;
        }
    }
}
