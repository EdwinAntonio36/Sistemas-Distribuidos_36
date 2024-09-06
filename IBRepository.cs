using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IBookRepository {

    public Task DeleteByIdAsync(BookModel book, CancellationToken cancellationToken);
    public Task<BookModel> GetByIdAsync(Guid bookId, CancellationToken cancellationToken);


}