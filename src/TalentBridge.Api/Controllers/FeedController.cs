using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("feed")]
[AllowAnonymous]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;
    private readonly ILogger<FeedController> _logger;

    public FeedController(IFeedService feedService, ILogger<FeedController> logger)
    {
        _feedService = feedService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> ObterFeed()
    {
        _logger.LogInformation("GET /feed");
        var resultado = await _feedService.GerarFeedXml();
        return resultado.Sucesso
            ? Content(resultado.Valor, "application/xml", Encoding.UTF8)
            : NotFound();
    }
}
