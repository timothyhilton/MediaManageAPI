using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Reflection.Metadata;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    [HttpGet("{str}")]
    public String Echo(String str)
    {
        return str; // this is just to test if the endpoint /video/ is accessible
    }

    [HttpPost]
    public ActionResult Post([FromForm] VideoModel video)
    {
        VideoArgModel videoArgs = JsonConvert.DeserializeObject<VideoArgModel>(video.videoArgs);
        Console.WriteLine(videoArgs.title);
        return StatusCode(StatusCodes.Status201Created);
    }
}
