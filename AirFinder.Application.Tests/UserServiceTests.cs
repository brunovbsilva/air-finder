﻿using AirFinder.Application.Email.Services;
using AirFinder.Application.Tests.Configuration;
using AirFinder.Application.Tests.Enums;
using AirFinder.Application.Tests.Mocks;
using AirFinder.Application.Users.Services;
using AirFinder.Domain.Common;
using AirFinder.Domain.People;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Tokens;
using AirFinder.Domain.Users;
using AirFinder.Domain.Users.Models.Requests;
using AirFinder.Domain.Users.Models.Responses;
using AirFinder.Infra.Security;
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
            CreateUserSetup(UserException.None);

            // Act
            var result = await _service.CreateUserAsync(request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData(UserException.LoginException)]
        [InlineData(UserException.CPFException)]
        [InlineData(UserException.EmailException)]
        public async Task CreateUserAsync_Exceptions(UserException userExeption)
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
            CreateUserSetup(UserException.None, true);

            // Act
            var result = await _service.CreateUserAdminAsync(request, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Theory]
        [InlineData(UserException.LoginException)]
        [InlineData(UserException.CPFException)]
        [InlineData(UserException.EmailException)]
        [InlineData(UserException.ForbiddenException)]
        public async Task CreateUserAdminAsync_Exceptions(UserException userExeption)
        {
            // Arrange
            var request = new UserAdminRequest();
            CreateUserSetup(userExeption, true);

            // Act
            var result = await _service.CreateUserAdminAsync(request, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            if (userExeption == UserException.ForbiddenException) NotificationAssert.ForbiddenNotification(_notification);
            else NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion

        #region LoginAsync
        [Fact]
        public async Task LoginAsync_ShouldLogin()
        {
            // Arrange
            var request = new LoginRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEnumerable().BuildMock());

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.IsType<LoginResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task LoginAsync_Exception()
        {
            // Arrange
            var request = new LoginRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEmptyEnumerable().BuildMock());

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region UpdatePasswordAsync
        [Fact]
        public async Task UpdatePasswordAsync_ShouldUpdate()
        {
            // Arrange
            var request = new UpdatePasswordRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEnumerable().BuildMock());

            // Act
            var result = await _service.UpdatePasswordAsync(It.IsAny<Guid>(), request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task UpdatePasswordAsync_Exception()
        {
            // Arrange
            var request = new UpdatePasswordRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEmptyEnumerable().BuildMock());

            // Act
            var result = await _service.UpdatePasswordAsync(It.IsAny<Guid>(), request);

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region SendTokenEmailAsync
        [Fact]
        public async Task SendTokenEmailAsync_ShouldSend()
        {
            // Arrange
            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEnumerable().BuildMock());

            // Act
            var result = await _service.SendTokenEmailAsync(It.IsAny<string>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        [Fact]
        public async Task SendTokenEmailAsync_Exception()
        {
            // Arrange
            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEmptyEnumerable().BuildMock());

            // Act
            var result = await _service.SendTokenEmailAsync(It.IsAny<string>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region VerifyTokenAsync
        [Fact]
        public async Task VerifyTokenAsync_ShouldVerify()
        {
            // Arrange
            var request = new VerifyTokenRequest();
            var userListMock = UserMocks.DefaultEnumerable().BuildMock();
            var tokenControlMock = new TokenControl() { IdUser = userListMock.FirstOrDefault()!.Id };

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(userListMock);
            _tokenRepository.Setup(x => x.GetByToken(It.IsAny<string>()))
                .ReturnsAsync(tokenControlMock);

            // Act
            var result = await _service.VerifyTokenAsync(request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Theory]
        [InlineData(VerifyTokenException.UserException)]
        [InlineData(VerifyTokenException.TokenException)]
        [InlineData(VerifyTokenException.InvalidTokenException)]
        public async Task VerifyTokenAsync_Exception(VerifyTokenException exception)
        {
            // Arrange
            var request = new VerifyTokenRequest();
            var tokenControlMock = TokenControlMocks.Default();

            if (exception == VerifyTokenException.InvalidTokenException) tokenControlMock.IdUser = Guid.NewGuid();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(
                    exception == VerifyTokenException.UserException ?
                    UserMocks.DefaultEmptyEnumerable().BuildMock() :
                    UserMocks.DefaultEnumerable().BuildMock()
                );
            if(exception != VerifyTokenException.TokenException)
                _tokenRepository.Setup(x => x.GetByToken(It.IsAny<string>()))
                    .ReturnsAsync(tokenControlMock);

            // Act
            var result = await _service.VerifyTokenAsync(request);

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region ChangePasswordAsync
        [Fact]
        public async Task ChangePasswordAsync_ShouldChange()
        {
            // Arrange
            var request = new ChangePasswordRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEnumerable().BuildMock());

            // Act
            var result = await _service.ChangePasswordAsync(request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Fact]
        public async Task ChangePasswordAsync_Exception()
        {
            // Arrange
            var request = new ChangePasswordRequest();

            _userRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(UserMocks.DefaultEmptyEnumerable().BuildMock());

            // Act
            var result = await _service.ChangePasswordAsync(request);

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region DeleteUserAsync
        [Fact]
        public async Task DeleteUserAsync_ShouldDelete()
        {
            // Arrange
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>()))
                .ReturnsAsync(UserMocks.Default());

            // Act
            var result = await _service.DeleteUserAsync(It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        [Fact]
        public async Task DeleteUserAsync_Exception()
        {
            // Act
            var result = await _service.DeleteUserAsync(It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region Private Methods
        private void CreateUserSetup(UserException userExeption, bool admin = false)
        {
            if(admin)
                _userRepository.SetupSequence(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                    .ReturnsAsync(userExeption != UserException.ForbiddenException)
                    .ReturnsAsync(userExeption == UserException.LoginException);
            else
                _userRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                    .ReturnsAsync(userExeption == UserException.LoginException);
            _personRepository.SetupSequence(x => x.AnyAsync(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(userExeption == UserException.CPFException)
                .ReturnsAsync(userExeption == UserException.EmailException);
        }
        #endregion
    }
}
