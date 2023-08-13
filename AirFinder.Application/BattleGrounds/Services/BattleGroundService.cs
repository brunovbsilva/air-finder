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

namespace AirFinder.Application.Battlegrounds.Services
{
    public class BattlegroundService : BaseService, IBattlegroundService
    {
        readonly IBattlegroundRepository _battlegroundRepository;
        readonly IUserRepository _userRepository;
        readonly IImgurService _imgurService;
        public BattlegroundService(
            INotification notification,
            IMailService mailService,
            IBattlegroundRepository battlegroundRepository,
            IUserRepository userRepository,
            IImgurService imgurService
        ) : base(notification, mailService) 
        {
            _battlegroundRepository = battlegroundRepository;
            _userRepository = userRepository;
            _imgurService = imgurService;
        }
        public async Task<BaseResponse> CreateBattleground(Guid id, CreateBattlegroundRequest request) => await ExecuteAsync(
            async () => {
                UploadResponse imgurResponse = await _imgurService.Upload(request.ImageBase64);
                var battleground = new Battleground(request.Name, imgurResponse!.Data.Link, request.CEP, request.Address, request.Number, request.City, request.State, request.Country, id);
                await _battlegroundRepository.InsertWithSaveChangesAsync(battleground);
                return new GenericResponse();
            }
        );

        public async Task<BaseResponse> DeleteBattleground(Guid id) => await ExecuteAsync(
            async () => {
                var battleground = await _battlegroundRepository.GetByIDAsync(id) ?? throw new NotFoundBattlegroundException();
                await _battlegroundRepository.DeleteAsync(id);
                return new GenericResponse();
            }
        );

        public async Task<GetBattlegroundsResponse> GetBattlegrounds(Guid id) => await ExecuteAsync(
            async () => {
                var user = await _userRepository.GetByIDAsync(id) ?? throw new NotFoundUserException();
                var battleground = await _battlegroundRepository.GetAll().Where(x => x.IdCreator == id).Select(x => (BattlegroundDto)x).ToListAsync();
                return new GetBattlegroundsResponse() { Battlegrounds = battleground };
            }
        );

        public async Task<BaseResponse> UpdateBattleground(Guid id, UpdateBattlegroundRequest request) => await ExecuteAsync(
            async () => { 
                var battleground = await _battlegroundRepository.GetByIDAsync(id) ?? throw new NotFoundBattlegroundException();

                battleground.Update(request);

                await _battlegroundRepository.UpdateWithSaveChangesAsync(battleground);
                return new GenericResponse(); 
            }
        );
    }
}
