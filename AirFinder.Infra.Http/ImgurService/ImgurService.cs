using AirFinder.Infra.Http.ImgurService.Responses;
using Newtonsoft.Json;
using System.Net.Http;

namespace AirFinder.Application.Imgur.Services
{
    public class ImgurService : IImgurService
    {
        private readonly HttpClient _httpClient;

        public ImgurService(HttpClient httpClient)
        {

            httpClient.DefaultRequestHeaders.Add("Authorization", "Client-ID d52de0ccfb3346d");
            _httpClient = httpClient;
        }
        public async Task<UploadResponse> Upload(string base64)
        {
            if (
                base64.Contains("data:image/png;base64,") ||
                base64.Contains("data:image/jpeg;base64,") ||
                base64.Contains("data:image/jpg;base64,") ||
                base64.Contains("data:image/gif;base64,")
            )
            {
                base64 = base64.Replace("data:image/png;base64,", "");
                base64 = base64.Replace("data:image/jpeg;base64,", "");
                base64 = base64.Replace("data:image/jpg;base64,", "");
                base64 = base64.Replace("data:image/gif;base64,", "");
            }
            else throw new Exception("Invalid image format");
            var content = new StringContent(base64);
            var response = await _httpClient.PostAsync("image", content).ConfigureAwait(false);
            var returned = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UploadResponse>(returned)!;
        }
    }
}
