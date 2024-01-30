using AirFinder.Application.Imgur.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People;
using AirFinder.Domain.People.Models.Requests;
using AirFinder.Domain.People.Models.Responses;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Infra.Security;
using AirFinder.Infra.Security.Request;

namespace AirFinder.Application.People.Services
{
    public class PersonService : BaseService, IPersonService
    {
        readonly IPersonRepository _personRepository;
        readonly IUserRepository _userRepository;
        readonly IImgurService _imgurService;
        readonly IJwtService _jwtService;
        public PersonService(
            INotification notification,
            IPersonRepository personRepository,
            IUserRepository userRepository,
            IImgurService imgurService,
            IJwtService jwtService
        ) : base(notification)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _imgurService = imgurService;
            _jwtService = jwtService;
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

        public async Task<UpdateProfileResponse> Update(UpdateProfileRequest request, Guid userId)
        => await ExecuteAsync(async () => {
            if (!request.IsValid()) throw new ArgumentNullException();

            User user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
            if (user.Person == null) throw new NotFoundPersonException();

            if (!String.IsNullOrEmpty(request.Name)) user.Person.UpdateName(request.Name);
            if (!String.IsNullOrEmpty(request.Email)) {
                if (await _personRepository.AnyAsync(x => x.Email == request.Email)) throw new EmailException();
                user.Person.UpdateEmail(request.Email);
            }
            if (!String.IsNullOrEmpty(request.Phone)) user.Person.UpdatePhone(request.Phone);
            if (request.Image != null && request.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    request.Image.CopyTo(ms);
                    user.Person.UpdateImageUrl((await _imgurService.Upload(Convert.ToBase64String(ms.ToArray()))).Data.Link);
                }
            }

            await _personRepository.UpdateWithSaveChangesAsync(user.Person);

            return new UpdateProfileResponse { Token = _jwtService.CreateToken(new CreateTokenRequest(user)) };
        });
    }
}
