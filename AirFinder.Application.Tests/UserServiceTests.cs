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

            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();
            var personEmptyListMock = new EnumerableQuery<Person>(new List<Person>()).BuildMock();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(userEmptyListMock);
            _personRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Person, bool>>>())).Returns(personEmptyListMock);

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
        public async Task CreateUserAsync_Exception(CreateUserException userExeption)
        {
            // Arrange
            var request = new UserRequest();

            var userListMock = new EnumerableQuery<User>(new List<User> { new User() }).BuildMock();
            var personListMock = new EnumerableQuery<Person>(new List<Person> { new Person() }).BuildMock();
            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();
            var personEmptyListMock = new EnumerableQuery<Person>(new List<Person>()).BuildMock();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(userExeption == CreateUserException.LoginException ? userListMock : userEmptyListMock);
            _personRepository.SetupSequence(x => x.Get(It.IsAny<Expression<Func<Person, bool>>>()))
                .Returns(userExeption == CreateUserException.CPFException ? personListMock : personEmptyListMock)
                .Returns(userExeption == CreateUserException.EmailException ? personListMock : personEmptyListMock);

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
            var id = It.IsAny<Guid>();

            var userListMock = new EnumerableQuery<User>(new List<User> { new User() }).BuildMock();
            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();
            var personEmptyListMock = new EnumerableQuery<Person>(new List<Person>()).BuildMock();

            _userRepository.SetupSequence(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(userListMock)
                .Returns(userEmptyListMock);
            _personRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Person, bool>>>())).Returns(personEmptyListMock);

            // Act
            var result = await _service.CreateUserAdminAsync(request, id);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task CreateUserAdminAsync_ForbiddenException()
        {
            // Arrange
            var request = new UserAdminRequest();
            var id = It.IsAny<Guid>();

            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(userEmptyListMock);

            // Act
            var result = await _service.CreateUserAdminAsync(request, id);

            // Assert
            Assert.Null(result);
            NotificationAssert.ForbiddenNotification(_notification);
        }

        [Theory]
        [InlineData(CreateUserException.LoginException)]
        [InlineData(CreateUserException.CPFException)]
        [InlineData(CreateUserException.EmailException)]
        public async Task CreateUserAdminAsync_Exception(CreateUserException userExeption)
        {
            // Arrange
            var request = new UserAdminRequest();
            var id = It.IsAny<Guid>();

            var userListMock = new EnumerableQuery<User>(new List<User> { new User() }).BuildMock();
            var personListMock = new EnumerableQuery<Person>(new List<Person> { new Person() }).BuildMock();
            var userEmptyListMock = new EnumerableQuery<User>(new List<User>()).BuildMock();
            var personEmptyListMock = new EnumerableQuery<Person>(new List<Person>()).BuildMock();

            _userRepository.SetupSequence(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(userListMock)
                .Returns(userExeption == CreateUserException.LoginException ? userListMock : userEmptyListMock);
            _personRepository.SetupSequence(x => x.Get(It.IsAny<Expression<Func<Person, bool>>>()))
                .Returns(userExeption == CreateUserException.CPFException ? personListMock : personEmptyListMock)
                .Returns(userExeption == CreateUserException.EmailException ? personListMock : personEmptyListMock);

            // Act
            var result = await _service.CreateUserAdminAsync(request, id);

            // Assert
            Assert.Null(result);
            NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion
    }
}
