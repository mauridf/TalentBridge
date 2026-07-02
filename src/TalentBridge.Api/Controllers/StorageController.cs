using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<StorageController> _logger;

    public StorageController(IStorageService storageService, ILogger<StorageController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("Upload")]
    [Authorize]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        _logger.LogInformation("POST /api/Storage/Upload - File: {FileName}", file?.FileName);
        if (file == null || file.Length == 0)
            return BadRequest(ResultadoDto<object>.Falha("ARQUIVO_INVALIDO", "Nenhum arquivo enviado."));

        using var stream = file.OpenReadStream();
        var resultado = await _storageService.UploadAsync(file.FileName, stream, file.ContentType);
        return resultado.Sucesso
            ? Ok(ResultadoDto<string>.Ok(resultado.Valor))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_UPLOAD", "Erro ao fazer upload do arquivo."));
    }

    [HttpGet("{blobName}")]
    [Authorize]
    public async Task<IActionResult> Download(string blobName)
    {
        var resultado = await _storageService.DownloadAsync(blobName);
        return resultado.Sucesso
            ? File(resultado.Valor, "application/octet-stream", blobName)
            : NotFound(ResultadoDto<object>.Falha("ARQUIVO_NAO_ENCONTRADO", "Arquivo não encontrado."));
    }

    [HttpDelete("{blobName}")]
    [Authorize]
    public async Task<IActionResult> Remover(string blobName)
    {
        _logger.LogInformation("DELETE /api/Storage/{BlobName}", blobName);
        var resultado = await _storageService.RemoverAsync(blobName);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Arquivo removido com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_REMOVER", "Erro ao remover arquivo."));
    }
}
