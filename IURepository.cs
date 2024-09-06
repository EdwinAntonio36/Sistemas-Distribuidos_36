using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository {
    Task<IList<UserModel>> GetAll(CancellationToken cancellationToken);
    Task<IList<UserModel>> GetAllByEmail(string email, CancellationToken cancellationToken);
    public Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken) ;

    public Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken);

    public Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken);

    public Task<bool> UpdateUser(Guid userId, string firstName, string lastName, DateTime birthday, CancellationToken cancellationToken);

}


