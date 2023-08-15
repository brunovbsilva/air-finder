using AirFinder.Application.Battlegrounds.Services;
using AirFinder.Application.Email.Services;
using AirFinder.Application.Imgur.Services;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Infra.Http.ImgurService.Dtos;
using AirFinder.Infra.Http.ImgurService.Responses;

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
        #endregion

        #region GetBattlegrounds
        #endregion

        #region UpdateBattleground
        #endregion
    }
}
