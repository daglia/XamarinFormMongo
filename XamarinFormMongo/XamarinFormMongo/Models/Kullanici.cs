using System;

namespace XamarinFormMongo.Models
{
    public class Kullanici
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        public bool IsValid()
        {
            if (KullaniciAdi.Length <= 0 || KullaniciAdi.Length > 6) return false;
            if (Sifre.Length <= 0 || Sifre.Length > 6) return false;
            if (KullaniciAdi.Contains(" ") || Sifre.Contains(" ")) return false;


            return true;
        }
    }
}
