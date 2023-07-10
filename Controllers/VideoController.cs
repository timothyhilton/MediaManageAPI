using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    private readonly IConfiguration _config;

    public VideoController(IConfiguration config)
    {
        _config = config;
    }

    // just helps to test if the endpoint /video/ is accessible
    [HttpGet("{str}")]
    public String Echo(String str)
    {
        return str; 
    }

    [HttpPost]
    [RequestSizeLimit(150_000_000)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromForm] VideoModel video)
    {
        string youtubeClientSecret = _config["youtubeClientSecret"];

        // calls VideoService to post the video
        await VideoService.PostVideo(video, youtubeClientSecret);
        return StatusCode(StatusCodes.Status201Created);
    }
}
