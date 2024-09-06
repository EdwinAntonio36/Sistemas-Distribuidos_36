using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SoapApi.Infrastructure;
using SoapApi.Mappers;
using SoapApi.Models;

namespace SoapApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly RelationalDbContext _dbContext;

    public BookRepository(RelationalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task DeleteByIdAsync(BookModel book, CancellationToken cancellationToken)
    {
        var bookEntity = book.ToEntity();
        _dbContext.Books.Remove(bookEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


  public async Task<BookModel> GetByIdAsync(Guid bookId, CancellationToken cancellationToken)
{
    var bookEntity = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
    return bookEntity.ToModel(); 
}

}
