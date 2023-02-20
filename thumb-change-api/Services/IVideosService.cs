using System;
using thumb_change_api.Models;

namespace thumb_change_api.Services
{
	public interface IVideosService
	{
        Task<List<VideoInfo>> GetVideosAsync();
    }
}
