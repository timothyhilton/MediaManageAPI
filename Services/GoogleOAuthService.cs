using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using MediaManageAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MediaManageAPI.Services;
public class GoogleOAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    public GoogleOAuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    public async Task HandleNewAuthCodeAsync(string authCode, System.Security.Claims.ClaimsPrincipal claimsUser){
        var user = await _userManager.FindByIdAsync(_userManager.GetUserId(claimsUser)); // possible bad implementation?

        var flow = GetFlow();

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            "test", //temporary
            authCode,
            "postmessage", // WHY DOES THIS WORK!?????
            CancellationToken.None
        );

        //user.YoutubeRefreshToken = tokenResponse.RefreshToken;

        //var result = await userManager.UpdateAsync(user);
        Console.WriteLine(tokenResponse.RefreshToken);
    }

    public UserCredential GetGoogleOAuthCredential(System.Security.Claims.ClaimsPrincipal claimsUser){
        var flow = GetFlow();

        var tokenResponse = new TokenResponse { 
            RefreshToken = claimsUser.FindFirstValue("YoutubeRefreshToken")
        };

        var credential = new UserCredential(
            flow, 
            claimsUser.FindFirstValue(ClaimTypes.NameIdentifier),
            tokenResponse
        );

        return credential;
    }

    private GoogleAuthorizationCodeFlow GetFlow(){
        return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer{
            ClientSecrets = new ClientSecrets
            {
                ClientId = "899123600204-92s1qc16e23p7ldjnc32cji6gsfpd1je.apps.googleusercontent.com",
                ClientSecret = _config["youtubeClientSecret"]
            },
            Scopes = new string[]{"https://www.googleapis.com/auth/youtube.upload"},
            DataStore = new FileDataStore("Store")
        });
    }
}