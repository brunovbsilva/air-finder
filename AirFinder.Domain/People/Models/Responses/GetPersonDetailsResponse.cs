using AirFinder.Domain.Common;

namespace AirFinder.Domain.People.Models.Responses
{
    public class GetPersonDetailsResponse : BaseResponse
    {
        public GetPersonDetailsResponse(
            string name,
            string email,
            string? imageUrl
        ) 
        {
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }

        public GetPersonDetailsResponse(Person person)
        {
            Name = person.Name;
            Email = person.Email;
            ImageUrl = person.ImageUrl;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
    }
}
