using XamarinFormMongo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormMongo.Services;

namespace XamarinFormMongo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
            //MongoTest();
        }

        private void MongoTest()
        {
            var db = MongoService.Db;
            var collection = db.GetCollection<Gorev>("gorevler");
            var models = new WriteModel<Gorev>[1];
            var gorev = new Gorev()
            {
                Aciklama = "Test girişi",
                BitisTarihi = DateTime.Now.AddDays(10),
                GorevAdi = "Test görevi"
            };
            models[0] = new ReplaceOneModel<Gorev>(new BsonDocument("_id", gorev.Id), gorev) { IsUpsert = true };
            collection.BulkWrite(models);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}