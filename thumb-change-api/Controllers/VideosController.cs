using Microsoft.AspNetCore.Mvc;
using mtb_race_api.Services;

namespace mtb_race_api.Controllers;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<VideosController> _logger;
    private readonly IVideosService _videosService;

    public VideosController(ILogger<VideosController> logger, IVideosService videosService)
    {
        _logger = logger;
        _videosService = videosService;
    }

    [HttpGet(Name = "GetVideos")]
    public async Task Get()
    {
        await _videosService.GetVideosAsync();
    }
}

