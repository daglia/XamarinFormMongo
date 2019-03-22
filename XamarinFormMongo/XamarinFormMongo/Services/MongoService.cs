using MongoDB.Driver;

namespace XamarinFormMongo.Services
{
    public static class MongoService
    {
        private static IMongoDatabase _db = null;
        public static IMongoDatabase Db
        {
            get
            {
                if (_db == null)
                {
                    var cred = MongoCredential.CreateCredential("xamarin", "form", "GxQX7aekRHWtnRD");
                    var sett = new MongoClientSettings
                    {
                        Server = new MongoServerAddress("ds119996.mlab.com", 19996),
                        Credential = cred
                    };
                    var client = new MongoClient(sett);
                    _db = client.GetDatabase("xamarin");
                }
                return _db;
            }
        }
    }
}
