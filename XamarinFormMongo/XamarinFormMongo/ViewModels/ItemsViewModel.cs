using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinFormMongo.Models;
using XamarinFormMongo.Views;

namespace XamarinFormMongo.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Gorev> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Gorev>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Gorev>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Gorev;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            MessagingCenter.Subscribe<ItemDetailPage, Gorev>(this, "UpdateItem", async (obj, item) =>
            {
                var newItem = item as Gorev;
                var update = Items.FirstOrDefault(x => x.Id == newItem.Id);
                update.YapilmaTarihi = item.YapilmaTarihi;
                await DataStore.UpdateItemAsync(newItem);
            });
            MessagingCenter.Subscribe<ItemDetailPage, Gorev>(this, "DeleteItem", async (obj, item) =>
            {
                await DataStore.DeleteItemAsync(item.Id);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}