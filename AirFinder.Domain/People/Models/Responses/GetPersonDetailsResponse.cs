using AirFinder.Domain.Common;
using AirFinder.Domain.People.Models.Dtos;

namespace AirFinder.Domain.People.Models.Responses
{
    public class GetPersonDetailsResponse : BaseResponse
    {
        public GetPersonDetailsResponse(
            Guid id,
            string name,
            string email,
            string? imageUrl
        ) 
        {
            Person = new PersonLimited(id, name, email, imageUrl);
        }

        public GetPersonDetailsResponse(Person person)
        {
            Person = new PersonLimited(person);
        }
        public PersonLimited Person { get; set; }
    }
}
