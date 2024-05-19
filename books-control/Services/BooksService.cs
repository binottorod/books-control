using BooksControl.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BooksControl.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        ArgumentNullException.ThrowIfNull(bookStoreDatabaseSettings);

        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
        _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync() => await _booksCollection.Find(_ => true).ToListAsync().ConfigureAwait(true);

    public async Task<Book?> GetAsync(string id) => await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(true);

    public async Task CreateAsync(Book newBook) => await _booksCollection.InsertOneAsync(newBook).ConfigureAwait(true);

    public async Task UpdateAsync(string id, Book updatedBook) => await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook).ConfigureAwait(true);

    public async Task DeleteAsync(string id) => await _booksCollection.DeleteOneAsync(x => x.Id == id).ConfigureAwait(true);


}
