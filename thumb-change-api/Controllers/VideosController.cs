using Microsoft.AspNetCore.Mvc;
using thumb_change_api.Services;
using thumb_change_api.Models;

namespace thumb_change_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly IVideosService _videosService;

    public VideosController(ILogger<VideosController> logger, IVideosService videosService)
    {
        _logger = logger;
        _videosService = videosService;
    }

    [HttpGet(Name = "GetVideos")]
    public async Task<ActionResult<List<VideoInfo>>> Get()
    {
        return await _videosService.GetVideosAsync();
    }
}
