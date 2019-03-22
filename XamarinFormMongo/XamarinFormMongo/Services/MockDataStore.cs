using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using XamarinFormMongo.Models;

namespace XamarinFormMongo.Services
{
    public class MockDataStore : IDataStore<Gorev>
    {
        List<Gorev> items;

        public MockDataStore()
        {
            GetData();
        }

        private void GetData()
        {
            items = new List<Gorev>();
            var db = MongoService.Db;
            var collection = db.GetCollection<Gorev>("gorevler");
            var findAll = collection.AsQueryable().ToCursorAsync(CancellationToken.None).Result;
            items = findAll.ToList();
        }
        public async Task<bool> AddItemAsync(Gorev item)
        {
            var db = MongoService.Db;
            var collection = db.GetCollection<Gorev>("gorevler");
            var wm = new WriteModel<Gorev>[1];
            wm[0] = new ReplaceOneModel<Gorev>(new BsonDocument("_id", item.Id), item) { IsUpsert = true };
            collection.BulkWrite(wm);
            GetData();
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Gorev item)
        {
            var db = MongoService.Db;
            var collection = db.GetCollection<Gorev>("gorevler");
            var wm = new WriteModel<Gorev>[1];
            wm[0] = new ReplaceOneModel<Gorev>(new BsonDocument("_id", item.Id), item) { IsUpsert = true };
            collection.BulkWrite(wm);
            //GetData();

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var db = MongoService.Db;
            var collection = db.GetCollection<Gorev>("gorevler");
            collection.DeleteOne(x => x.Id == id);

            return await Task.FromResult(true);
        }

        public async Task<Gorev> GetItemAsync(Guid id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Gorev>> GetItemsAsync(bool forceRefresh = false)
        {
            GetData();
            return await Task.FromResult(items);
        }
    }
}