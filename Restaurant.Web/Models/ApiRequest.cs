using static Restaurant.Web.SD;
namespace Restaurant.Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
        public string AceessToken { get;  set; }

    }
}
