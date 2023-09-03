using AirFinder.Application.Email.Services;
using AirFinder.Application.Tests.Configuration;
using AirFinder.Application.Tests.Enums;
using AirFinder.Application.Users.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People;
using AirFinder.Domain.People.Enums;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Enums;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Infra.Security;
using AirFinder.Infra.Utils.Constants;
using Azure;
using System.Linq;
using System.Linq.Expressions;

namespace AirFinder.Application.Tests
{
    public class UserServiceTests
    {
        readonly Mock<INotification> _notification;
        readonly Mock<IUserRepository> _userRepository;
        readonly Mock<IPersonRepository> _personRepository;
        readonly Mock<ITokenRepository> _tokenRepository;
        readonly Mock<IJwtService> _jwtService;
        readonly Mock<IMailService> _mailService;
        readonly UserService _service;

        public UserServiceTests()
        {
            _notification = new Mock<INotification>();
            _userRepository = new Mock<IUserRepository>();
            _personRepository = new Mock<IPersonRepository>();
            _tokenRepository = new Mock<ITokenRepository>();
            _jwtService = new Mock<IJwtService>();
            _mailService = new Mock<IMailService>();

            _service = new UserService(
                _notification.Object,
                _userRepository.Object,
                _personRepository.Object,
                _tokenRepository.Object,
                _jwtService.Object,
                _mailService.Object
            );
        }

        #region CreateUserAsync
        [Fact]
        public async Task CreateUserAsync_ShouldCreate()
        {
            // Arrange
            var request = new UserRequest();
            CreateUserSetup(CreateUserException.None);

            // Act
            var result = await _service.CreateUserAsync(request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData(CreateUserException.LoginException)]
        [InlineData(CreateUserException.CPFException)]
        [InlineData(CreateUserException.EmailException)]
        public async Task CreateUserAsync_Exceptions(CreateUserException userExeption)
        {
            // Arrange
            var request = new UserRequest();
            CreateUserSetup(userExeption);

            // Act
            var result = await _service.CreateUserAsync(request);

            // Assert
            Assert.Null(result);
            NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion

        #region CreateUserAdminAsync
        [Fact]
        public async Task CreateUserAdminAsync_ShouldCreate()
        {
            // Arrange
            var request = new UserAdminRequest();
            CreateUserSetup(CreateUserException.None, true);

            // Act
            var result = await _service.CreateUserAdminAsync(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData(CreateUserException.LoginException)]
        [InlineData(CreateUserException.CPFException)]
        [InlineData(CreateUserException.EmailException)]
        [InlineData(CreateUserException.ForbiddenException)]
        public async Task CreateUserAdminAsync_Exceptions(CreateUserException userExeption)
        {
            // Arrange
            var request = new UserAdminRequest();
            CreateUserSetup(userExeption, true);

            // Act
            var result = await _service.CreateUserAdminAsync(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            if (userExeption == CreateUserException.ForbiddenException) NotificationAssert.ForbiddenNotification(_notification);
            else NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion

        #region LoginAsync
        #endregion

        #region UpdatePasswordAsync
        #endregion

        #region SendTokenEmailAsync
        #endregion

        #region VerifyTokenAsync
        #endregion

        #region ChangePasswordAsync
        #endregion

        #region DeleteUserAsync
        #endregion

        #region Private Methods
        private void CreateUserSetup(CreateUserException userExeption, bool admin = false)
        {
            var userListMock = new EnumerableQuery<User>(new List<User> { new User() }).BuildMock();
            var personListMock = new EnumerableQuery<Person>(new List<Person> { new Person() }).BuildMock();
            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();
            var personEmptyListMock = new EnumerableQuery<Person>(new List<Person>()).BuildMock();

            if(admin)
                _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                    .Returns(userExeption == CreateUserException.ForbiddenException ? userEmptyListMock : userListMock);

            _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userExeption == CreateUserException.LoginException ? true : false);
            _personRepository.SetupSequence(x => x.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(userExeption == CreateUserException.CPFException ? true : false)
                .ReturnsAsync(userExeption == CreateUserException.EmailException ? true : false);
        }
        #endregion
    }
}
