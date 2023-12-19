using AirFinder.Application.Imgur.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People;
using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;

namespace AirFinder.Application.People.Services
{
    public class PersonService : BaseService, IPersonService
    {
        readonly IPersonRepository _personRepository;
        readonly IUserRepository _userRepository;
        readonly IImgurService _imgurService;
        public PersonService(
            INotification notification,
            IPersonRepository personRepository,
            IUserRepository userRepository,
            IImgurService imgurService
        ) : base(notification)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _imgurService = imgurService;
        }

        public async Task<SearchPeopleResponse> Search(SearchPeopleRequest request)
        => await ExecuteAsync(async () => {
            if (String.IsNullOrEmpty(request.Search) || request.Search.Length < 3) throw new SearchPeopleException();
            return await _personRepository.Search(request);
        });

        public async Task<GetPersonDetailsResponse> Details(Guid personId)
        => await ExecuteAsync(async () => {
            var person = await _personRepository.GetByIDAsync(personId) ?? throw new NotFoundPersonException();
            return new GetPersonDetailsResponse(person);
        });

        public async Task<BaseResponse> Update(UpdateProfileRequest request, Guid userId)
        => await ExecuteAsync(async () => {
            if (!request.IsValid()) throw new ArgumentNullException();

            User user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            Person person = await _personRepository.GetByIDAsync(user.IdPerson) ?? throw new NotFoundPersonException();

            if (!String.IsNullOrEmpty(request.Name)) person.UpdateName(request.Name);
            if (!String.IsNullOrEmpty(request.Email)) {
                if (await _personRepository.AnyAsync(x => x.Email == request.Email)) throw new EmailException();
                person.UpdateEmail(request.Email);
            }
            if (!String.IsNullOrEmpty(request.Phone)) person.UpdatePhone(request.Phone);
            if (!String.IsNullOrEmpty(request.Image)) person.UpdateImageUrl((await _imgurService.Upload(request.Image)).Data.Link);

            await _personRepository.UpdateWithSaveChangesAsync(person);

            return new GenericResponse();
        });
    }
}
