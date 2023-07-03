using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoInfoModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string fileExtension { get; set; }
        public string accessToken { get; set; }
        
    }
}