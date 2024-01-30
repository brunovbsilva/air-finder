//using AirFinder.Application.People.Services;
//using AirFinder.Domain.People;
//using AirFinder.Domain.People.Models.Requests;
//using AirFinder.Domain.People.Models.Responses;

//namespace AirFinder.API.Tests
//{
//    public class PersonControllerTests
//    {
//        readonly TestConfiguration _configuration;
//        readonly Mock<IPersonService> _personService;
//        readonly Mock<INotification> _notification;
//        readonly Mock<HttpContext> _httpContext;
//        readonly PersonController _controller;

//        public PersonControllerTests()
//        {
//            _personService = new Mock<IPersonService>();
//            _notification = new Mock<INotification>();
//            _httpContext = new Mock<HttpContext>();

//            _configuration = new TestConfiguration(_notification, _httpContext);

//            _controller = new PersonController(_notification.Object, _personService.Object)
//            {
//                ControllerContext = new ControllerContext { HttpContext = _httpContext.Object }
//            };
//        }

//        #region SearchPeople
//        [Fact]
//        public async Task SearchPeople_ShouldReturnOk()
//        {
//            // Arrange
//            var request = It.IsAny<SearchPeopleRequest>();
//            var response = new SearchPeopleResponse(It.IsAny<List<Person>>());
//            _personService.Setup(x => x.Search(It.IsAny<SearchPeopleRequest>())).ReturnsAsync(response);

//            // Act
//            var result = await _controller.SearchPeople(request);

//            // Assert
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Theory]
//        [InlineData(ENotificationType.BadRequestError)]
//        public async Task SearchPeople_Errors(ENotificationType notificationType)
//        {
//            // Arrange
//            var request = It.IsAny<SearchPeopleRequest>();
//            _configuration.SetupNotification(notificationType);

//            // Act
//            var result = await _controller.SearchPeople(request);

//            // Assert
//            _configuration.NotificationsAsserts(notificationType, result);
//        }
//        #endregion

//        #region UpdateProfile
//        [Fact]
//        public async Task UpdateProfile_ShouldReturnOk()
//        {
//            // Arrange
//            var request = It.IsAny<UpdateProfileRequest>();
//            var response = It.IsAny<GenericResponse>();
//            _personService.Setup(x => x.Update(It.IsAny<UpdateProfileRequest>(), It.IsAny<Guid>())).ReturnsAsync(response);

//            // Act
//            var result = await _controller.UpdateProfile(request);

//            // Assert
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Theory]
//        [InlineData(ENotificationType.BadRequestError)]
//        [InlineData(ENotificationType.NotAllowed)]
//        public async Task UpdateProfile_Errors(ENotificationType notificationType)
//        {
//            // Arrange
//            var request = It.IsAny<UpdateProfileRequest>();
//            _configuration.SetupNotification(notificationType);

//            // Act
//            var result = await _controller.UpdateProfile(request);

//            // Assert
//            _configuration.NotificationsAsserts(notificationType, result);
//        }
//        #endregion

//        #region Details
//        [Fact]
//        public async Task Details_ShouldReturnOk()
//        {
//            // Arrange
//            var request = It.IsAny<Guid>();
//            var response = It.IsAny<GetPersonDetailsResponse>();
//            _personService.Setup(x => x.Details(It.IsAny<Guid>())).ReturnsAsync(response);

//            // Act
//            var result = await _controller.Details(request);

//            // Assert
//            Assert.IsType<OkObjectResult>(result);
//        }

//        [Theory]
//        [InlineData(ENotificationType.BadRequestError)]
//        public async Task Details_Errors(ENotificationType notificationType)
//        {
//            // Arrange
//            var request = It.IsAny<Guid>();
//            _configuration.SetupNotification(notificationType);

//            // Act
//            var result = await _controller.Details(request);

//            // Assert
//            _configuration.NotificationsAsserts(notificationType, result);
//        }
//        #endregion
//    }
//}
