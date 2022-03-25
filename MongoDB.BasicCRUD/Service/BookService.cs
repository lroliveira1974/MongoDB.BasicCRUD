
using Microsoft.Extensions.Options;
using MongoDB.BasicCRUD.Model;
using MongoDB.Driver;

namespace MongoDB.BasicCRUD.Service
{
    public class BookService
    {
        // INJETA DEPENDENCIA DO MONGODB
        private readonly IMongoCollection<BookModel> _booksCollection;

        public BookService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<BookModel>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<BookModel>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<BookModel?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(BookModel newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, BookModel updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
