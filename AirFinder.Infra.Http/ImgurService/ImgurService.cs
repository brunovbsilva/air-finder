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
        public async Task<UploadResponse?> Upload(string base64)
        {
            var result = new UploadResponse();
            try {
                var content = new StringContent(base64);
                var response = await _httpClient.PostAsync("image", content).ConfigureAwait(false);
                var returned = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<UploadResponse>(returned);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
