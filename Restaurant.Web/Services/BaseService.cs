using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.IServices;

namespace Restaurant.Web.Services
{
    public class BaseService:IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDto();
            this.httpClient= httpClient;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("ResturantApi");
                HttpRequestMessage massage = new HttpRequestMessage();
                massage.Headers.Add("Accept","application/json");
                massage.RequestUri = new Uri(apiRequest.ApiUrl);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data is not null)
                {
                    massage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),Encoding.UTF8,"application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AceessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",apiRequest.AceessToken);
                }
                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        massage.Method=HttpMethod.Post;
                        break; 
                    case SD.ApiType.DELETE:
                        massage.Method=HttpMethod.Delete;
                        break; 
                    case SD.ApiType.PUT:
                        massage.Method=HttpMethod.Put;
                        break;
                    default: 
                        massage.Method=HttpMethod.Get;
                        break;
                }

                apiResponse =await client.SendAsync(massage);
               var apiContent= await apiResponse.Content.ReadAsStringAsync();
               var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
               return apiResponseDto;
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    Message = "Error",
                    ErrorMassages = new List<string>
                    {
                        Convert.ToString(e.Message)
                    },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }
    }
}
