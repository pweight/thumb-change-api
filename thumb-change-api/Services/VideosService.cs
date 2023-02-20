using System;
using System.Net.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using thumb_change_api.Models;

namespace thumb_change_api.Services
{
	public class VideosService : IVideosService
	{
		private readonly HttpClient _httpClient;

		public VideosService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<VideoInfo>> GetVideosAsync()
		{
			UserCredential credential;
			using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
			{
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.FromStream(stream).Secrets, // TODO make this Async
					// This OAuth 2.0 access scope allows for read-only access to the authenticated 
					// user's account, but not other types of account access.
					new[] { YouTubeService.Scope.Youtubepartner },
					"user",
					CancellationToken.None,
					new FileDataStore(this.GetType().ToString())
				);
            }

			var youTubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = this.GetType().ToString()
			});

			var channelsListRequest = youTubeService.Channels.List("contentDetails");
			channelsListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();

			var videoInfoList = new List<VideoInfo>();

			foreach (var channel in channelsListResponse.Items)
			{
				// From the API response, extract the playlist ID that identifies the list
				// of videos uploaded to the authenticated user's channel.
				var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

				Console.WriteLine($"Videos in list {uploadsListId}");

				var nextPageToken = string.Empty;
				while (nextPageToken != null)
				{
                    var playlistItemsListRequest = youTubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadsListId;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel.
                    var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        // Print information about each video.
                        Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
						var videoInfo = new VideoInfo
						{
							Name = playlistItem.Snippet.Title,
							Id = playlistItem.Snippet.ResourceId.VideoId
						};

						videoInfoList.Add(videoInfo);
                    }

                    nextPageToken = playlistItemsListResponse.NextPageToken;
                }
            }

			return videoInfoList;
        }
	}
}
