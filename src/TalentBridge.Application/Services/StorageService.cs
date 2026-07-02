using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Application.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;

    public StorageService(ILogger<StorageService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<string>> UploadAsync(string fileName, Stream content, string contentType)
    {
        _logger.LogWarning("StorageService.UploadAsync não implementado. FileName: {FileName}", fileName);
        throw new NotImplementedException("StorageService.UploadAsync ainda não foi implementado.");
    }

    public Task<ResultadoDto<Stream>> DownloadAsync(string blobName)
    {
        _logger.LogWarning("StorageService.DownloadAsync não implementado. BlobName: {BlobName}", blobName);
        throw new NotImplementedException("StorageService.DownloadAsync ainda não foi implementado.");
    }

    public Task<ResultadoDto<bool>> RemoverAsync(string blobName)
    {
        _logger.LogWarning("StorageService.RemoverAsync não implementado. BlobName: {BlobName}", blobName);
        throw new NotImplementedException("StorageService.RemoverAsync ainda não foi implementado.");
    }

    public Task<ResultadoDto<string>> GetUrlAsync(string blobName)
    {
        _logger.LogWarning("StorageService.GetUrlAsync não implementado. BlobName: {BlobName}", blobName);
        throw new NotImplementedException("StorageService.GetUrlAsync ainda não foi implementado.");
    }
}
