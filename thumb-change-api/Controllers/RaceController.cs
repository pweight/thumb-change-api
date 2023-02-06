using Microsoft.AspNetCore.Mvc;

namespace mtb_race_api.Controllers;

[ApiController]
[Route("[controller]")]
public class RaceController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private List<Race> Races = new List<Race>();

    private readonly ILogger<RaceController> _logger;

    public RaceController(ILogger<RaceController> logger)
    {
        _logger = logger;

        Races.AddRange(Enumerable.Range(1, 5).Select(index => new Race
        {
            Date = DateTimeOffset.Now.AddDays(index),
            Name = Summaries[Random.Shared.Next(Summaries.Length)],
            Location = Summaries[Random.Shared.Next(Summaries.Length)],
            Region = Summaries[Random.Shared.Next(Summaries.Length)],
        })
        .ToList());
    }

    [HttpGet(Name = "GetRaces")]
    public ActionResult<IEnumerable<Race>> Get()
    {
        Console.WriteLine(Races.Count);
        _logger.LogInformation(Races.Count.ToString());
        return Ok(Races);
    }

    [HttpPost(Name = "AddRace")]
    public ActionResult<Race> Post(Race raceRequest)
    {
        var race = new Race
        {
            Date = raceRequest.Date,
            Name = raceRequest.Name,
            Location = raceRequest.Location,
            Region = raceRequest.Region
        };

        Races.Add(race);
        _logger.LogInformation(Races.Count.ToString());
        Console.WriteLine(Races.Count);

        return Ok(race);
    }
}

