using Abp.Domain.Uow;
using AirFinder.Domain.People;
using AirFinder.Domain.People.Models.Dtos;
using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirFinder.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public PersonRepository(IUnitOfWork unitOfWork) : base(unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<SearchPeopleResponse> Search(SearchPeopleRequest request)
        {
            var tbPeople = _unitOfWork.Context.Set<Person>().AsNoTracking();
            var search = request.Search?.Trim().ToLower();

            var query = (from p in tbPeople select p)
                .Where(x => x.Name.ToLower().Contains(search) || x.Email.ToLower().Contains(search) || x.Phone.Contains(search))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Email);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / request.ItemsPerPage);
            if (request.PageIndex >= totalPages) request.PageIndex = totalPages - 1;
            if (request.PageIndex < 0) request.PageIndex = 0;

            var peopleList = await query
                .Skip(request.ItemsPerPage * (request.PageIndex))
                .Take(request.ItemsPerPage)
                .ToListAsync();

            return new SearchPeopleResponse(peopleList.Select(x => new PersonLimited(x)));
        }
    }
}
