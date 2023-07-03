using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string VideoInfos { get; set; } 
        // VideoInfos should always be a JSON-stringified VideoArgModel
        // the above is done to avoid ugly code in the frontend
        public IFormFile File { get; set; }
    }
}