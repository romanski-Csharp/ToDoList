using MongoDB.Driver;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class ToDoService
    {
        private readonly IMongoCollection<ToDoItem> _items;

        public ToDoService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("ToDoDb");
            _items = database.GetCollection<ToDoItem>("ToDoItems");
        }

        public async Task<List<ToDoItem>> GetAsync() =>
            await _items.Find(_ => true).ToListAsync();

        public async Task<ToDoItem> GetAsync(string id) =>
            await _items.Find(i => i.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ToDoItem item) =>
            await _items.InsertOneAsync(item);

        public async Task UpdateAsync(string id, ToDoItem item) =>
            await _items.ReplaceOneAsync(i => i.Id == id, item);

        public async Task DeleteAsync(string id) =>
            await _items.DeleteOneAsync(i => i.Id == id);
    }
}
