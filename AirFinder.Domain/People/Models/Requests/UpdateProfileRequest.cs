using Microsoft.AspNetCore.Http;

namespace AirFinder.Domain.People.Models.Requests
{
    public class UpdateProfileRequest
    {
        public UpdateProfileRequest() {}
        public UpdateProfileRequest(string? name, string? email, string? phone, IFormFile? image)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Image = image;
        }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public IFormFile? Image { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name) || !String.IsNullOrEmpty(Email) || !String.IsNullOrEmpty(Phone) || Image != null;
        }
    }
}
