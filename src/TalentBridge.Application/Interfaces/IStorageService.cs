using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public interface IStorageService
{
    Task<ResultadoDto<string>> UploadAsync(string fileName, Stream content, string contentType);
    Task<ResultadoDto<Stream>> DownloadAsync(string blobName);
    Task<ResultadoDto<bool>> RemoverAsync(string blobName);
    Task<ResultadoDto<string>> GetUrlAsync(string blobName);
}
