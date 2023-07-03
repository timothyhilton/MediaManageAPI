using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string VideoArgs { get; set; } 
        // this should always be a JSON-stringified VideoArgModel
        // the above is done to avoid ugly code in the frontend
        public IFormFile File { get; set; }
    }
}