using Restaurant.Web.Models;

namespace Restaurant.Web.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync <T>(ApiRequest apiRequest);
        
    }
}
