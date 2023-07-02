using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string AccessToken { get; set; }
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}