using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediaManageAPI.Models;
using MediaManageAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace MediaManageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly TokenService _tokenService;
    private readonly UsersContext _context;
    private readonly GoogleOAuthService _googleOAuthService;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, UsersContext context, GoogleOAuthService googleOAuthService)
    {
        _userManager = userManager;
        _config = config;
        _tokenService = new TokenService(config);
        _context = context;
        _googleOAuthService = googleOAuthService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _userManager.CreateAsync(
            new ApplicationUser { UserName = request.Username, Email = request.Email },
            request.Password
        );
        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }
        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
            return Unauthorized();
        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();
        return Ok(new AuthResponse
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }

    [HttpPost, Authorize]
    [Route("GAuthCode")]
    public async Task<IActionResult> PostGoogleAuthCode([FromBody] string authCode){
        return await _googleOAuthService.HandleNewAuthCodeAsync(authCode, User);
    }
}
