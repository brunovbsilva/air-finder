using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.Common;
using AirFinder.Domain.Users;

namespace AirFinder.Domain.BattleGrounds
{
    public class BattleGround : BaseModel
    {
        public BattleGround(string name, string imageUrl, string cep, string address, int number, string city, string state, string country, Guid idCreator)
        {
            Name = name;
            ImageUrl = imageUrl;
            CEP = cep;
            Address = address;
            Number = number;
            City = city;
            State = state;
            Country = country;
            IdCreator = idCreator;
            Creator = null;
        }
        public BattleGround() { }
        public string Name { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
        public string CEP { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public int Number { get; set; } = 0;
        public string City { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public Guid IdCreator { get; set; }
        public virtual User? Creator { get; set; }

        public void Update(UpdateBattleGroundRequest request)
        {
            Name = request.Name;
            ImageUrl = request.ImageUrl;
            CEP = request.CEP;
            Address = request.Address;
            Number = request.Number;
            City = request.City;
            State = request.State;
            Country = request.Country;
        }
    }
}
