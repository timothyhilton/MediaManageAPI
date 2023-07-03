using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public VideoArgModel VideoArgs { get; set; } // this should be a JSON-stringified VideoArgModel
        public IFormFile File { get; set; }
    }
}