using AirFinder.Domain.People;

namespace AirFinder.Domain.Users.Models.Responses
{
    public class DetailsResponse
    {
        public DetailsResponse(
            string name,
            string email,
            string? imageUrl
        )
        {
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }

        public DetailsResponse(Person person)
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
