using AirFinder.Domain.Common;

namespace AirFinder.Domain.Battlegrounds.Models.Dtos
{
    public class BattlegroundDto : BaseModel
    {
        public string Name { get; set; } = String.Empty;
        public string ImageUrl { get; set; } = String.Empty;
        public string CEP { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public int Number { get; set; } = 0;
        public string City { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string Country { get; set; } = String.Empty;
        public Guid IdCreator { get; set; }

        public static implicit operator BattlegroundDto(Battleground bg)
        {
            if (bg == null) return new BattlegroundDto();
            return new BattlegroundDto
            {
                Id = bg.Id,
                Name = bg.Name,
                ImageUrl = bg.ImageUrl,
                CEP = bg.CEP,
                Address = bg.Address,
                Number = bg.Number,
                City = bg.City,
                State = bg.State,
                Country = bg.Country,
                IdCreator = bg.IdCreator
            };
        }
    }
}
