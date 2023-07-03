using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult Post(VideoRequestModel videoRequest)
    {
        try
        {
            Console.WriteLine(videoRequest.accessToken);

            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
