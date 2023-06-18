using AirFinder.Application.Email.Services;
using AirFinder.Application.Imgur.Services;
using AirFinder.Domain.BattleGrounds;
using AirFinder.Domain.BattleGrounds.Models.Dtos;
using AirFinder.Domain.BattleGrounds.Models.Requests;
using AirFinder.Domain.BattleGrounds.Models.Responses;
using AirFinder.Domain.Common;
using AirFinder.Domain.SeedWork.Notification;
using AirFinder.Domain.Users;
using AirFinder.Infra.Http.ImgurService.Responses;
using Microsoft.EntityFrameworkCore;

namespace AirFinder.Application.BattleGrounds.Services
{
    public class BattleGroundService : BaseService, IBattleGroundService
    {
        readonly IBattleGroundRepository _battleGroundRepository;
        readonly IUserRepository _userRepository;
        readonly IImgurService _imgurService;
        public BattleGroundService(
            INotification notification,
            IMailService mailService,
            IBattleGroundRepository battleGroundRepository,
            IUserRepository userRepository,
            IImgurService imgurService
        ) : base(notification, mailService) 
        {
            _battleGroundRepository = battleGroundRepository;
            _userRepository = userRepository;
            _imgurService = imgurService;
        }
        public async Task<BaseResponse?> CreateBattleGround(Guid id, CreateBattleGroundRequest request) => await ExecuteAsync(
            async () => {
                UploadResponse imgurResponse = await _imgurService.Upload(request.ImageBase64);
                var battleGround = new BattleGround(request.Name, imgurResponse!.Data.Link, request.CEP, request.Address, request.Number, request.City, request.State, request.Country, id);
                await _battleGroundRepository.InsertWithSaveChangesAsync(battleGround);
                return new GenericResponse();
            }
        );

        public async Task<BaseResponse?> DeleteBattleGround(Guid id) => await ExecuteAsync(
            async () => {
                var battleGround = await _battleGroundRepository.GetByIDAsync(id) ?? throw new ArgumentException("BattleGround not found");
                await _battleGroundRepository.DeleteAsync(id);
                return new GenericResponse();
            }
        );

        public async Task<GetBattleGroundResponse?> GetBattleGrounds(Guid id) => await ExecuteAsync(
            async () => {
                var user = await _userRepository.GetByIDAsync(id) ?? throw new ArgumentException("User not found");
                var battleGround = await _battleGroundRepository.GetAll().Where(x => x.IdCreator == id).Select(x => (BattleGroundDto)x).ToListAsync();
                return new GetBattleGroundResponse() { Battlegrounds = battleGround };
            }
        );

        public async Task<BaseResponse?> UpdateBattleGround(Guid id, UpdateBattleGroundRequest request) => await ExecuteAsync(
            async () => { 
                var battleGround = await _battleGroundRepository.GetByIDAsync(id) ?? throw new ArgumentException("BattleGround not found");

                battleGround.Name = request.Name;
                battleGround.ImageUrl = request.ImageUrl;
                battleGround.CEP = request.CEP;
                battleGround.Address = request.Address;
                battleGround.Number = request.Number;
                battleGround.City = request.City;
                battleGround.State = request.State;
                battleGround.Country = request.Country;

                await _battleGroundRepository.UpdateWithSaveChangesAsync(battleGround);
                return new GenericResponse(); 
            }
        );
    }
}
