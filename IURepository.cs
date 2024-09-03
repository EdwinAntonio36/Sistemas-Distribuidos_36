using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository {
    public Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken) ;

        Task<IList<UserModel>> GetAll(CancellationToken cancellationToken);
    Task<IList<UserModel>> GetAllByEmail(string email, CancellationToken cancellationToken);


}


