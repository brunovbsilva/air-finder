using AirFinder.Infra.Http.ImgurService.Responses;

namespace AirFinder.Application.Imgur.Services
{
    public interface IImgurService
    {
        Task<UploadResponse> Upload(string request);
    }
}
