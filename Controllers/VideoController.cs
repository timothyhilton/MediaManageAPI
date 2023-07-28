using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private GoogleOAuthService googleOAuthService;
    public VideoController(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _userManager = userManager;
        googleOAuthService = new GoogleOAuthService(_config, _userManager);
    }

    // just helps to test if the endpoint /video/ is accessible
    [HttpGet("{str}")]
    public String Echo(String str)
    {
        return str;
    }

    [HttpPost, Authorize]
    [RequestSizeLimit(150_000_000)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromForm] VideoModel video)
    {
        await VideoService.PostVideo(video, googleOAuthService.GetGoogleOAuthCredential(User));

        return StatusCode(StatusCodes.Status201Created);
    }
}
