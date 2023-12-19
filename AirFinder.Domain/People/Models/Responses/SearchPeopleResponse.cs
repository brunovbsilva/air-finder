using AirFinder.Domain.Common;

namespace AirFinder.Domain.People.Models.Responses
{
    public class SearchPeopleResponse : BaseResponse
    {
        public SearchPeopleResponse(IEnumerable<Person> people)
        {
            People = people;
        }
        public IEnumerable<Person> People { get; set; }
    }
}
