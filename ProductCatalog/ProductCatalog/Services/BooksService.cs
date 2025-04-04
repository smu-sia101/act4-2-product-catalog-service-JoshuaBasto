namespace ProductCatalog.Services
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using ProductCatalog.Models;

    public class BooksService
    {
        private readonly IMongoCollection<ProductModel> _booksCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<ProductModel>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<ProductModel>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<ProductModel?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ProductModel newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, ProductModel updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}