using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto reponseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.reponseModel = new ResponseDto();
            this.httpClient = httpClient;
        }
        public void Dispose()
        {
            //Garbage Collection
            GC.SuppressFinalize(true);
        }

        //generic API request package details
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MangoAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                //include access token code in header
                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }
                
                HttpResponseMessage apiResponse = null;
                message.Method = apiRequest.ApiType switch
                {
                    SD.ApiType.GET => HttpMethod.Get,
                    SD.ApiType.POST => HttpMethod.Post,
                    SD.ApiType.PUT => HttpMethod.Put,
                    SD.ApiType.DELETE => HttpMethod.Delete,
                   // _ => throw new ArgumentException(nameof(apiRequest.ApiType))
                };

                //switch (apiRequest.ApiType)
                //{
                //    case SD.ApiType.POST:
                //        message.Method = HttpMethod.Post;
                //        break;
                //    case SD.ApiType.PUT:
                //        message.Method = HttpMethod.Put;
                //        break;
                //    case SD.ApiType.DELETE:
                //        message.Method = HttpMethod.Delete;
                //        break;
                //    default:
                //        message.Method = HttpMethod.Get;
                //        break;
                //}
                apiResponse = await client.SendAsync(message);
                
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDto;
            }
            catch (Exception e)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }
    }
}
