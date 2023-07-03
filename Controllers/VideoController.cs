using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    Random random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    StringBuilder builder = new StringBuilder();

[HttpGet("{str}")]
    public String Echo(String str)
    {
        return str; // this is just to test if the endpoint /video/ is accessible
    }

    [HttpPost]
    [RequestSizeLimit(150_000_000)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult Post([FromForm] VideoModel video)
    {
        VideoInfoModel videoInfos = new VideoInfoModel();

        try { videoInfos = JsonConvert.DeserializeObject<VideoInfoModel>(video.VideoInfos); } // get the video information from the JSON in "video"
        catch { return StatusCode(StatusCodes.Status400BadRequest); } // return bad request if it doesn't match the values required

        try // saves the video file to a file with a random string as the name
        {
            for (int i = 0; i < 20; i++)
            {
                builder.Append(chars[(int)(random.NextDouble() * chars.Length)]);
            }
            string randomString = builder.ToString();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/escrow", randomString+"."+videoInfos.fileExtension);
            
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                video.File.CopyTo(stream);
            }



            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
