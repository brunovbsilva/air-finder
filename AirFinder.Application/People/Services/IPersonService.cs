using AirFinder.Domain.Common;
using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;

namespace AirFinder.Application.People.Services
{
    public interface IPersonService
    {
        Task<SearchPeopleResponse> Search(SearchPeopleRequest request);
        Task<BaseResponse> Update(UpdateProfileRequest request, Guid userId);
        Task<GetPersonDetailsResponse> Details(Guid personId);
    }
}
