using MediaManageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MediaManageAPI.Services;
public static class VideoService
{
    public static async Task PostVideo(VideoModel video, string accessToken)
    {
        Console.WriteLine("video upload started");
        var videoTitle = video.title;
        var videoDescription = video.description;
        var videoTags = video.tags;
        var videoCategoryId = "1"; // temporary

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var videoContent = video.file;

            var metadata = new
            {
                snippet = new
                {
                    title = videoTitle,
                    description = videoDescription,
                    tags = videoTags,
                    categoryId = videoCategoryId
                },
                status = new
                {
                    privacyStatus = "public" // or "private" or "unlisted"
                }
            };

            var metadataJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(metadata);
            var metadataContent = new StringContent(metadataJsonString);
            metadataContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.googleapis.com/upload/youtube/v3/videos?part=snippet,status"))
            {
                ByteArrayContent byteContent = new ByteArrayContent(videoContent);
                requestMessage.Content = byteContent;

                using (var responseMessage = await httpClient.SendAsync(requestMessage))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error uploading video: {responseMessage.ReasonPhrase}");
                        return;
                    }

                    Console.WriteLine("Video uploaded successfully.");
                }
            }
        }
    }
}