using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XamarinFormMongo.Models;

namespace XamarinFormMongo.Services
{
    public class AzureDataStore : IDataStore<Gorev>
    {
        HttpClient client;
        IEnumerable<Gorev> items;

        public AzureDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}/");

            items = new List<Gorev>();
        }


        public async Task<IEnumerable<Gorev>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"api/item");
                items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Gorev>>(json));
            }

            return items;
        }

        public async Task<Gorev> GetItemAsync(Guid id)
        {
            var json = await client.GetStringAsync($"api/item/{id}");
            return await Task.Run(() => JsonConvert.DeserializeObject<Gorev>(json));

        }

        public async Task<bool> AddItemAsync(Gorev item)
        {
            if (item == null)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(Gorev item)
        {
            if (item == null)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var response = await client.DeleteAsync($"api/item/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}