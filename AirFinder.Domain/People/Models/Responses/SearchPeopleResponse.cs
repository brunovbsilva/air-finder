using AirFinder.Domain.Common;
using AirFinder.Domain.People.Models.Dtos;

namespace AirFinder.Domain.People.Models.Responses
{
    public class SearchPeopleResponse : BaseResponse
    {
        public SearchPeopleResponse(IEnumerable<PersonLimited> people)
        {
            People = people;
        }
        public IEnumerable<PersonLimited> People { get; set; }
    }
}
