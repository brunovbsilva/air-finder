using AirFinder.Application.Battlegrounds.Services;
using AirFinder.Application.Email.Services;
using AirFinder.Application.Imgur.Services;
using AirFinder.Application.Tests.Configuration;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Battlegrounds.Models.Dtos;
using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Infra.Http.ImgurService.Dtos;
using AirFinder.Infra.Http.ImgurService.Responses;
using AirFinder.Infra.Utils.Constants;

namespace AirFinder.Application.Tests
{
    public class BattlegroundServiceTests
    {
        readonly Mock<INotification> _notification;
        readonly Mock<IMailService> _mailService;
        readonly Mock<IBattlegroundRepository> _battlegroundRepository;
        readonly Mock<IUserRepository> _userRepository;
        readonly Mock<IImgurService> _imgurService;
        readonly BattlegroundService _service;

        public BattlegroundServiceTests()
        {
            _notification = new Mock<INotification>();
            _mailService = new Mock<IMailService>();
            _battlegroundRepository = new Mock<IBattlegroundRepository>();
            _userRepository = new Mock<IUserRepository>();
            _imgurService = new Mock<IImgurService>();

            _service = new BattlegroundService(
                _notification.Object,
                _mailService.Object,
                _battlegroundRepository.Object,
                _userRepository.Object,
                _imgurService.Object
            );
        }

        #region CreateBattleground
        [Fact]
        public async Task CreateBattleground_ShouldCreate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateBattlegroundRequest();
            var imgurResponse = new UploadResponse { Data = new DataDto { Link = "teste.com" } };
            _imgurService.Setup(x => x.Upload(It.IsAny<string>())).ReturnsAsync(imgurResponse);

            // Act
            var result = await _service.CreateBattleground(userId, request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
        #endregion

        #region DeleteBattleground
        [Fact]
        public async Task DeleteBattleground_ShouldDeleteBattleground()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var battleground = MockedBattleground(userId);
            _battlegroundRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(battleground);

            // Act
            var result = await _service.DeleteBattleground(userId, It.IsAny<Guid>());

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            _battlegroundRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBattleground_NotFoundBattlegroundException()
        {
            // Act
            var result = await _service.DeleteBattleground(It.IsAny<Guid>(), It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            _battlegroundRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            NotificationAssert.BadRequestNotification(_notification);
        }

        [Fact]
        public async Task DeleteBattleground_MethodNotAllowedException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var battleground = MockedBattleground(It.IsAny<Guid>());
            _battlegroundRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(battleground);

            // Act
            var result = await _service.DeleteBattleground(userId, It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion

        #region GetBattlegrounds
        [Fact]
        public async Task GetBattlegrounds_ShouldReturnBattlegrounds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var battlegrounds = new List<Battleground> { MockedBattleground(userId) }.AsQueryable().BuildMock();
            _userRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(new User());
            _battlegroundRepository.Setup(x => x.GetAll()).Returns(battlegrounds);

            // Act
            var result = await _service.GetBattlegrounds(userId);

            // Assert
            Assert.IsType<GetBattlegroundsResponse>(result); 
            Assert.True(result.Success);
            Assert.Null(result.Error);
            Assert.Equivalent(battlegrounds.Select(x => (BattlegroundDto)x).ToList(), result.Battlegrounds);
        }

        [Fact]
        public async Task GetBattlegrounds_NotFoundUserException()
        {
            // Act
            var result = await _service.GetBattlegrounds(It.IsAny<Guid>());

            // Assert
            Assert.Null(result);
            NotificationAssert.BadRequestNotification(_notification);
        }
        #endregion

        #region UpdateBattleground
        [Fact]
        public async Task UpdateBattleground_ShouldUpdate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UpdateBattlegroundRequest();
            _battlegroundRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(MockedBattleground(userId));

            // Act
            var result = await _service.UpdateBattleground(userId, It.IsAny<Guid>(), request);

            // Assert
            Assert.IsType<GenericResponse>(result);
            Assert.True(result.Success);
            Assert.Null(result.Error);
            _battlegroundRepository.Verify(x => x.UpdateWithSaveChangesAsync(It.IsAny<Battleground>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBattleground_NotFoundBattlegroundException()
        {
            // Act
            var result = await _service.UpdateBattleground(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<UpdateBattlegroundRequest>());

            // Assert
            Assert.Null(result);
            _battlegroundRepository.Verify(x => x.UpdateWithSaveChangesAsync(It.IsAny<Battleground>()), Times.Never);
            NotificationAssert.BadRequestNotification(_notification);
        }

        [Fact]
        public async Task UpdateBattleground_MethodNotAllowedException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var battleground = MockedBattleground(It.IsAny<Guid>());
            _battlegroundRepository.Setup(x => x.GetByIDAsync(It.IsAny<Guid>())).ReturnsAsync(battleground);

            // Act
            var result = await _service.UpdateBattleground(userId, It.IsAny<Guid>(), It.IsAny<UpdateBattlegroundRequest>());

            // Assert
            Assert.Null(result);
            NotificationAssert.MethodNotAllowedNotification(_notification);
        }
        #endregion

        #region private methods
        private static Battleground MockedBattleground(Guid id)
        {
            return new Battleground(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                id
            );
        }
        #endregion
    }
}
