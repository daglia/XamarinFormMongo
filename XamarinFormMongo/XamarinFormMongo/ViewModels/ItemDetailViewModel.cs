using System;

using XamarinFormMongo.Models;

namespace XamarinFormMongo.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Gorev Item { get; set; }
        public ItemDetailViewModel(Gorev item = null)
        {
            Title = item?.GorevAdi;
            Item = item;
        }
    }
}
