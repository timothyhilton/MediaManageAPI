using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Model
{
    public class VideoModel
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}