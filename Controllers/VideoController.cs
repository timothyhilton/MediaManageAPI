using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MediaManageAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class VideoController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    public VideoController(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config;
        _userManager = userManager;
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
        var credential = GoogleOAuthService.GetGoogleOAuthCredential(
            User,
            _userManager,
            _config["youtubeClientSecret"]!
        );

        await VideoService.PostVideo(
            video, 
            credential
        );

        return StatusCode(StatusCodes.Status201Created);
    }
}
