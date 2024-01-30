//using AirFinder.Application.Imgur.Services;
//using AirFinder.Application.People.Services;
//using AirFinder.Application.Tests.Configuration;
//using AirFinder.Application.Tests.Mocks;
//using AirFinder.Domain.Common;
//using AirFinder.Domain.People;
//using AirFinder.Domain.People.Models.Requests;
//using AirFinder.Domain.People.Models.Responses;
//using AirFinder.Domain.SeedWork.Notification;
//using AirFinder.Domain.Users;
//using AirFinder.Infra.Http.ImgurService.Dtos;
//using AirFinder.Infra.Http.ImgurService.Responses;
//using System.Linq.Expressions;

//namespace AirFinder.Application.Tests
//{
//    public class PersonServiceTests
//    {
//        readonly Mock<IPersonRepository> _personRepository;
//        readonly Mock<IUserRepository> _userRepository;
//        readonly Mock<IImgurService> _imgurService;
//        readonly Mock<INotification> _notification;
//        readonly PersonService _service;

//        public PersonServiceTests()
//        {
//            _notification = new Mock<INotification>();
//            _personRepository = new Mock<IPersonRepository>();
//            _userRepository = new Mock<IUserRepository>();
//            _imgurService = new Mock<IImgurService>();

//            _service = new PersonService(
//                _notification.Object,
//                _personRepository.Object,
//                _userRepository.Object,
//                _imgurService.Object
//            );
//        }

//        #region Search
//        [Fact]
//        public async Task Search_Success()
//        {
//            // Arrange
//            var request = new SearchPeopleRequest("mock request");
//            var response = new SearchPeopleResponse(PersonMocks.DefaultEmptyEnumerable());
//            _personRepository.Setup(x => x.Search(It.IsAny<SearchPeopleRequest>())).ReturnsAsync(response);

//            // Act
//            var result = await _service.Search(request);

//            // Assert
//            Assert.IsType<SearchPeopleResponse>(result);
//            Assert.True(result.Success);
//            Assert.Null(result.Error);
//        }

//        [Fact]
//        public async Task Search_Less_Than_3_Characteres()
//        {
//            // Arrange
//            var request = new SearchPeopleRequest("a");

//            // Act
//            var result = await _service.Search(request);

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        [Fact]
//        public async Task Search_Empty_Search()
//        {
//            // Arrange
//            var request = new SearchPeopleRequest("");

//            // Act
//            var result = await _service.Search(request);

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        #endregion

//        #region Details
//        [Fact]
//        public async Task Details_Success()
//        {
//            // Arrange
//            var person = PersonMocks.Default();
//            _personRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(person);

//            // Act
//            var result = await _service.Details(It.IsAny<Guid>());

//            // Assert
//            Assert.IsType<GetPersonDetailsResponse>(result);
//            Assert.True(result.Success);
//            Assert.Null(result.Error);
//        }
//        [Fact]
//        public async Task Details_Not_Found()
//        {
//            // Arrange
//            _personRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync((Person)null);

//            // Act
//            var result = await _service.Details(It.IsAny<Guid>());

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        #endregion

//        #region Update
//        [Theory]
//        [InlineData(null, null, null, "mock image")]
//        [InlineData(null, null, "mock phone", null)]
//        [InlineData(null, "mock email", null, null)]
//        [InlineData("mock name", null, null, null)]
//        public async Task Update_Success(string? name, string? email, string? phone, string? image)
//        {
//            // Arrange
//            var request = new UpdateProfileRequest(name, email, phone, image);
//            var imgurResponse = new UploadResponse { Data = new DataDto { Link = "teste.com" } };

//            _personRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(false);
//            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(UserMocks.Default());
//            _personRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(PersonMocks.Default());
//            _imgurService.Setup(x => x.Upload(It.IsAny<string>())).ReturnsAsync(imgurResponse);

//            // Act
//            var result = await _service.Update(request, It.IsAny<Guid>());

//            // Assert
//            Assert.IsType<GenericResponse>(result);
//            Assert.True(result.Success);
//            Assert.Null(result.Error);
//        }
//        [Fact]
//        public async Task Update_Invalid_Request()
//        {
//            // Arrange
//            var request = new UpdateProfileRequest(null, null, null, null);

//            // Act
//            var result = await _service.Update(request, It.IsAny<Guid>());

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        [Fact]
//        public async Task Update_Not_Found_User()
//        {
//            // Arrange
//            var request = new UpdateProfileRequest("mock name", "mock email", "mock phone", "mock image");
//            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync((User)null);

//            // Act
//            var result = await _service.Update(request, It.IsAny<Guid>());

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        [Fact]
//        public async Task Update_Not_Found_Person()
//        {
//            // Arrange
//            var request = new UpdateProfileRequest("mock name", "mock email", "mock phone", "mock image");
//            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(UserMocks.Default());
//            _personRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync((Person)null);

//            // Act
//            var result = await _service.Update(request, It.IsAny<Guid>());

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.BadRequestNotification(_notification);
//        }
//        [Fact]
//        public async Task Update_Email_Already_Exists()
//        {
//            // Arrange
//            var request = new UpdateProfileRequest("mock name", "mock email", "mock phone", "mock image");
//            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(UserMocks.Default());
//            _personRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(PersonMocks.Default());
//            _personRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(true);

//            // Act
//            var result = await _service.Update(request, It.IsAny<Guid>());

//            // Assert
//            Assert.Null(result);
//            NotificationAssert.MethodNotAllowedNotification(_notification);
//        }
//        #endregion
//    }
//}
