namespace AirFinder.Domain.People.Models.Dtos
{
    public class PersonLimited
    {
        public PersonLimited() { }
        public PersonLimited(Guid id, string name, string email, string? imageUrl)
        {
            Id = id;
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }
        public PersonLimited(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            Email = person.Email;
            ImageUrl = person.ImageUrl;
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageUrl { get; set; }   
    }
}
