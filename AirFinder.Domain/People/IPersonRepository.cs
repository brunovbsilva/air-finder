using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;

namespace AirFinder.Domain.People
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<SearchPeopleResponse> Search(SearchPeopleRequest request);
    }
}
