using System.ServiceModel;
using Microsoft.OpenApi.Validations.Rules;
using SoapApi.Dtos;

namespace SoapApi.Contracts;

[ServiceContract]

public interface IBookContract{
    [OperationContract]
    public Task<bool> DeleteBookById(Guid bookId, CancellationToken cancellationToken);


}
