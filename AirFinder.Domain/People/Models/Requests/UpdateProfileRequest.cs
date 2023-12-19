namespace AirFinder.Domain.People.Models.Requests
{
    public class UpdateProfileRequest
    {
        public UpdateProfileRequest() {}
        public UpdateProfileRequest(string? name, string? email, string? phone, string? image)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Image = image;
        }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Name) || !String.IsNullOrEmpty(Email) || !String.IsNullOrEmpty(Phone) || !String.IsNullOrEmpty(Image);
        }
    }
}
