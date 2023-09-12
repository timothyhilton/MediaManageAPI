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
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MediaManageAPI.Services;
public class VideoService
{
    public static async Task PostVideo(VideoModel video, UserCredential credential)
    {
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var ytVideo = new Video();
            ytVideo.Snippet = new VideoSnippet();
            ytVideo.Snippet.Title = video.title;
            ytVideo.Snippet.Description = video.description;
            ytVideo.Snippet.Tags = new string[] { "testtag1", "testtag2" }; // temporary
            ytVideo.Snippet.CategoryId = "1"; // temporary 
            ytVideo.Status = new VideoStatus();
            ytVideo.Status.PrivacyStatus = "unlisted"; // temporary, or "private" or "public"

            using (var fileStream = video.File.OpenReadStream())
            {
                var videosInsertRequest = youtubeService.Videos.Insert(ytVideo, "snippet,status", fileStream, "video/*");
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

        void videosInsertRequest_ResponseReceived(Video ytVideo)
        {
            Console.WriteLine("Video id '{0}' was successfully uploaded.", ytVideo.Id);
        }
    }

    public static List<YoutubeVideo> FetchVideos(UserCredential credential)
    {
        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
        });

        var channelsListRequest = youtubeService.Channels.List("contentDetails");
        channelsListRequest.Mine = true;

        var channelsListResponse = channelsListRequest.Execute();
        int VideoCount = 1;
        List<YoutubeVideo> list = new List<YoutubeVideo>();
        foreach (var channel in channelsListResponse.Items)
        {
            var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
            var nextPageToken = "";
            while (nextPageToken != null)
            {
                var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                playlistItemsListRequest.PlaylistId = uploadsListId;
                playlistItemsListRequest.MaxResults = 20;
                playlistItemsListRequest.PageToken = nextPageToken;
                // Retrieve the list of videos uploaded to the authenticated user's channel.  
                var playlistItemsListResponse = playlistItemsListRequest.Execute();
                foreach (var playlistItem in playlistItemsListResponse.Items)
                {
                    list.Add(new YoutubeVideo
                    {
                        Title = playlistItem.Snippet.Title,
                        Description = playlistItem.Snippet.Description,
                        ImageUrl = playlistItem.Snippet.Thumbnails.High.Url,
                        VideoSource = "https://www.youtube.com/embed/" + playlistItem.Snippet.ResourceId.VideoId,
                        VideoId = playlistItem.Snippet.ResourceId.VideoId,
                        VideoOwnerChannelTitle = playlistItem.Snippet.VideoOwnerChannelTitle

                    }); ;

                    VideoCount++;
                }
                nextPageToken = playlistItemsListResponse.NextPageToken;
            }

        }
        return list;
    }
}
