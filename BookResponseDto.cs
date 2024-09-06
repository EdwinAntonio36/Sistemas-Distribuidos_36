using System.Runtime.Serialization;

namespace SoapApi.Dtos;

[DataContract]
public class BookResponseDto{
    [DataMember]
    public Guid BookId {get; set;}


}