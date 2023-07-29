using Google.Apis.Auth.OAuth2;
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
    private readonly GoogleOAuthService _googleOAuthService;
    public VideoController(IConfiguration config, UserManager<ApplicationUser> userManager, GoogleOAuthService googleOAuthService)
    {
        _config = config;
        _userManager = userManager;
        _googleOAuthService = googleOAuthService;
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
    public async Task<IActionResult> Post([FromForm] VideoModel video)
    {
        IActionResult credentialResult = _googleOAuthService.GetGoogleOAuthCredential(User);
    
        if (credentialResult is OkObjectResult okObjectResult && okObjectResult.Value is UserCredential credential){
            await VideoService.PostVideo(video, credential);
            
            return StatusCode(StatusCodes.Status201Created);
        }

        return credentialResult;
    }
}
