﻿using MediaManageAPI.Models;
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

    // setup for random file name generation
    Random random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    StringBuilder builder = new StringBuilder();

    [HttpGet("{str}")] // this is just to test if the endpoint /video/ is accessible
    public String Echo(String str)
    {
        return str; 
    }

    [HttpPost]
    [RequestSizeLimit(150_000_000)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public ActionResult Post([FromForm] VideoModel video)
    {
        VideoInfoModel videoInfos = new VideoInfoModel();
        string youtubeClientSecret = _config["youtubeClientSecret"];
        Console.WriteLine(youtubeClientSecret);
        Console.ReadLine();

        try { videoInfos = JsonConvert.DeserializeObject<VideoInfoModel>(video.VideoInfos); } // get the video information from the JSON in "video"
        catch { return StatusCode(StatusCodes.Status400BadRequest); } // return bad request if it doesn't match the values required

        try // saves the video file to a file with a random string as the name
        {
            for (int i = 0; i < 20; i++)
            {
                builder.Append(chars[(int)(random.NextDouble() * chars.Length)]);
            }
            string randomString = builder.ToString();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/escrow", randomString + "." + videoInfos.fileExtension);
            
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                video.File.CopyTo(stream);
            }

            VideoService.PostVideo(videoInfos, path, youtubeClientSecret);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            Console.WriteLine("VideoController: request failed");
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
