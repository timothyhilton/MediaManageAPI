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
    public static async Task HandleNewAuthCodeAsync(string authCode, System.Security.Claims.ClaimsPrincipal claimsUser, UserManager<ApplicationUser> userManager, IConfiguration config){
        var user = await userManager.FindByIdAsync(userManager.GetUserId(claimsUser)); // possible bad implementation?

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer{
            ClientSecrets = new ClientSecrets
            {
                ClientId = "899123600204-92s1qc16e23p7ldjnc32cji6gsfpd1je.apps.googleusercontent.com",
                ClientSecret = config["youtubeClientSecret"]
            },
            Scopes = new string[]{"https://www.googleapis.com/auth/youtube.upload"},
            DataStore = new FileDataStore("Store")
        });;

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(
            claimsUser.FindFirstValue(ClaimTypes.NameIdentifier),
            authCode,
            "postmessage", // WHY DOES THIS WORK!?????
            CancellationToken.None
        );

        user.YoutubeRefreshToken = tokenResponse.RefreshToken;

        var result = await userManager.UpdateAsync(user);
    }

    public static UserCredential GetGoogleOAuthCredential(System.Security.Claims.ClaimsPrincipal claimsUser, UserManager<ApplicationUser> userManager, IConfiguration config){
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer{
            ClientSecrets = new ClientSecrets
            {
                ClientId = "899123600204-92s1qc16e23p7ldjnc32cji6gsfpd1je.apps.googleusercontent.com",
                ClientSecret = config["youtubeClientSecret"]
            },
            Scopes = new string[]{"https://www.googleapis.com/auth/youtube.upload"},
            DataStore = new FileDataStore("Store")
        });

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
}