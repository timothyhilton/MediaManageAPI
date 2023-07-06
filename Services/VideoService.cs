using Newtonsoft.Json;
using MediaManageAPI.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.YouTube.v3;
using System.Reflection;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;

namespace MediaManageAPI.Services;
public class VideoService
{
    public static async void PostVideo(VideoInfoModel videoInfos, string videoPath, string youtubeClientSecret)
    {
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "899123600204-92s1qc16e23p7ldjnc32cji6gsfpd1je.apps.googleusercontent.com",
                    ClientSecret = youtubeClientSecret
                },
                Scopes = new string[]{"https://www.googleapis.com/auth/youtube.upload"},
                DataStore = new FileDataStore("Store")
            });

            var credential = new UserCredential(flow, "test", await flow.ExchangeCodeForTokenAsync("test", videoInfos.authCode, "", CancellationToken.None));

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = videoInfos.title;
            video.Snippet.Description = videoInfos.description;
            video.Snippet.Tags = new string[] { "testtag1", "testtag2" }; // temporary
            video.Snippet.CategoryId = "1"; // temporary 
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "unlisted"; // temporary, or "private" or "public"
            var filePath = videoPath; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}
