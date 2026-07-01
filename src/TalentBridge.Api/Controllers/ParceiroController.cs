using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para gestão de parceiros (admin)
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class ParceiroController : ControllerBase
{
    /// <summary>
    /// Lista todos os parceiros (placeholder - implementação completa futura)
    /// </summary>
    [HttpGet]
    public IActionResult Listar()
    {
        return Ok(new { mensagem = "Endpoint de parceiros - em desenvolvimento" });
    }
}
