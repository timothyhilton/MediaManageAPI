namespace MediaManageAPI.Models
{
    public class VideoModel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
        public string fileExtension { get; set; }
        public byte[] file { get; set; }
    }
}