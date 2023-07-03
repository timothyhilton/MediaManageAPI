using Microsoft.AspNetCore.Http;

namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string videoArgs {  get; set; }
        public IFormFile file { get; set; }
    }
}