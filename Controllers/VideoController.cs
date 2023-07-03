﻿using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;

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
    [RequestSizeLimit(150_000_000)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult Post([FromForm] VideoModel video)
    {
        VideoInfoModel videoInfos = new VideoInfoModel();

        try { videoInfos = JsonConvert.DeserializeObject<VideoInfoModel>(video.VideoArgs); }
        catch { return StatusCode(StatusCodes.Status400BadRequest); }

        try
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/escrow", "test."+ videoInfos.fileExtension);
            
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
