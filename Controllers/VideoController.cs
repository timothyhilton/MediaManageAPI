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

        try // saves the video file to a file with a random string as the name
        {
            // generate random file name string
            for (int i = 0; i < 20; i++)
            {
                builder.Append(chars[(int)(random.NextDouble() * chars.Length)]);
            }
            string randomString = builder.ToString();

            // create path for video file to be saved
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/escrow", randomString + "." + video.fileExtension);
            
            // saves video
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                video.File.CopyTo(stream);
            }

            // calls VideoService to post the video
            await VideoService.PostVideo(video, path, youtubeClientSecret);
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
