using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormMongo.Models;
using XamarinFormMongo.Services;

namespace XamarinFormMongo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginRegisterPage : ContentPage
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<Kullanici> collection;
        public LoginRegisterPage()
        {
            InitializeComponent();
            registerLayout.IsVisible = swRegister.IsToggled;
            loginLayout.WidthRequest = DeviceDisplay.MainDisplayInfo.Width - 100;
            swRegister.Toggled += (sender, e) => { registerLayout.IsVisible = e.Value; };
            db = MongoService.Db;
            collection = db.GetCollection<Kullanici>("kullanicilar");
            InitMyComponents();
        }

        private void InitMyComponents()
        {
            btnregister.Clicked += async (sender, e) =>
            {
                if (txtRegSifre.Text != txtRegSifreTekrar.Text)
                {
                    await DisplayAlert("Kayıt hatası", "Şifreler uyumsuz", "Ok");
                    return;
                }
                var yeniKullanici = new Kullanici()
                {
                    KullaniciAdi = txtRegKullaniciAdi.Text,
                    Sifre = txtRegSifre.Text
                };
                if (!yeniKullanici.IsValid())
                {
                    await DisplayAlert("Kayıt hatası", "Kullanıcı adı veya şifre kurallara uymamaktadır", "Ok");
                }
                else
                {
                    var wm = new WriteModel<Kullanici>[1];
                    wm[0] = new ReplaceOneModel<Kullanici>(new BsonDocument("_id", yeniKullanici.Id), yeniKullanici) { IsUpsert = true };
                    collection.BulkWrite(wm);
                    await DisplayAlert("Kayıt Başarılı", $"Hoşgeldin {yeniKullanici.KullaniciAdi} giriş yapabilirsin", "Ok");
                    swRegister.IsToggled = false;
                }
            };
            btnLogin.Clicked += async (sender, e) =>
            {
                var kullanici = collection.AsQueryable().FirstOrDefault(x =>
                    x.KullaniciAdi == txtKullaniciAdi.Text && x.Sifre == txtSifre.Text);

                if (kullanici == null)
                {
                    await DisplayAlert("Giriş hatası", "Kullanıcı adı veya şifre hatalı", "Ok");
                    return;
                }
                await DisplayAlert("Giriş Başarılı", $"Hoşgeldin {kullanici.KullaniciAdi}", "Ok");
            };
        }
    }
}