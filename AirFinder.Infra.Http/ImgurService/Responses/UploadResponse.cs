using AirFinder.Infra.Http.ImgurService.Dtos;

namespace AirFinder.Infra.Http.ImgurService.Responses
{
    public class UploadResponse
    {
        public DataDto Data { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }
}
