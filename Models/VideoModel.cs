namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public IFormFile File { get; set; }
    }
}