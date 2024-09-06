using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SoapApi.Infrastructure;
using SoapApi.Mappers;
using SoapApi.Models;

namespace SoapApi.Repositories;

public class UserRespository : IUserRepository{

    private readonly RelationalDbContext _dbContext;
    public UserRespository(RelationalDbContext dbContext){


        _dbContext = dbContext;

    }


    public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken) {


        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        
        return user.ToModel();
    }
    public async Task<IList<UserModel>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().ToListAsync(cancellationToken);
        return users.Select(user => user.ToModel()).ToList();
    }
    public async Task<IList<UserModel>> GetAllByEmail(string email, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking()
                    .Where(user => user.Email.Contains(email))
                    .ToListAsync(cancellationToken);
        
        return users.Select(user => user.ToModel()).ToList();
    }


    public async Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        _dbContext.Users.Remove(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        userEntity.Id = Guid.NewGuid();
        await _dbContext.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return userEntity.ToModel();      
    }

public async Task<bool> UpdateUser(Guid id,string firstName,string lastName,DateTime birthday, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);

        if (user == null)
        {
            return false; 
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Birthday = birthday;

        _dbContext.Users.Update(user);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true; 
        }
        catch (Exception)
        {
            
            return false; 
        }
    }
}