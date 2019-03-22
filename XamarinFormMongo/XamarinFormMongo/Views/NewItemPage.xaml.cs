using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinFormMongo.Models;

namespace XamarinFormMongo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Gorev Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();



            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item = new Gorev()
            {
                GorevAdi = txtGorevAdi.Text,
                Aciklama = txtAciklama.Text,
                BitisTarihi = dpBitis.Date
            };
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}