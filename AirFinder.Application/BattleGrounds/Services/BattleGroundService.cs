using AirFinder.Application.Email.Services;
using AirFinder.Application.Imgur.Services;
using AirFinder.Domain.Battlegrounds;
using AirFinder.Domain.Battlegrounds.Models.Dtos;
using AirFinder.Domain.Battlegrounds.Models.Requests;
using AirFinder.Domain.Battlegrounds.Models.Responses;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Infra.Http.ImgurService.Responses;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;

namespace AirFinder.Application.Battlegrounds.Services
{
    public class BattlegroundService : BaseService, IBattlegroundService
    {
        readonly IBattlegroundRepository _battlegroundRepository;
        readonly IUserRepository _userRepository;
        readonly IImgurService _imgurService;
        public BattlegroundService(
            INotification notification,
            IBattlegroundRepository battlegroundRepository,
            IUserRepository userRepository,
            IImgurService imgurService
        ) : base(notification) 
        {
            _battlegroundRepository = battlegroundRepository;
            _userRepository = userRepository;
            _imgurService = imgurService;
        }
        public async Task<BaseResponse> CreateBattleground(Guid userId, CreateBattlegroundRequest request) => await ExecuteAsync(
            async () => {
                var battleground = new Battleground(request);
                battleground.SetCreator(userId);
                if (!string.IsNullOrEmpty(request.ImageBase64)) battleground.SetImage((await _imgurService.Upload(request.ImageBase64)).Data.Link);

                await _battlegroundRepository.InsertWithSaveChangesAsync(battleground);
                return new GenericResponse();
            }
        );

        public async Task<BaseResponse> DeleteBattleground(Guid userId, Guid id) => await ExecuteAsync(
            async () => {
                var battleground = await _battlegroundRepository.GetByIDAsync(id) ?? throw new NotFoundBattlegroundException();
                if (battleground.IdCreator != userId) throw new MethodNotAllowedException();
                await _battlegroundRepository.DeleteAsync(id);
                return new GenericResponse();
            }
        );

        public async Task<GetBattlegroundsResponse> GetBattlegrounds(Guid userId) => await ExecuteAsync(
            async () => {
                var user = await _userRepository.GetByIDAsync(userId) ?? throw new NotFoundUserException();
                var battlegrounds = await _battlegroundRepository.GetAll().Where(x => x.IdCreator == userId).Select(x => (BattlegroundDto)x).ToListAsync();
                return new GetBattlegroundsResponse() { Battlegrounds = battlegrounds };
            }
        );

        public async Task<BaseResponse> UpdateBattleground(Guid userId, Guid id, UpdateBattlegroundRequest request) => await ExecuteAsync(
            async () => { 
                var battleground = await _battlegroundRepository.GetByIDAsync(id) ?? throw new NotFoundBattlegroundException();
                if (battleground.IdCreator != userId) throw new MethodNotAllowedException();

                battleground.Update(request);
                if (!string.IsNullOrEmpty(request.ImageBase64)) battleground.SetImage((await _imgurService.Upload(request.ImageBase64)).Data.Link);

                await _battlegroundRepository.UpdateWithSaveChangesAsync(battleground);
                return new GenericResponse(); 
            }
        );
    }
}
