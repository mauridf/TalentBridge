using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.Storage;

public class MinioSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
}

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioSettings _settings;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IOptions<MinioSettings> settings, ILogger<MinioStorageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        _minioClient = new MinioClient()
            .WithEndpoint(_settings.Endpoint)
            .WithCredentials(_settings.AccessKey, _settings.SecretKey);

        if (_settings.UseSsl)
            _minioClient = _minioClient.WithSSL();

        _minioClient = _minioClient.Build();
    }

    public async Task<ResultadoDto<string>> UploadAsync(string fileName, Stream content, string contentType)
    {
        try
        {
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_settings.BucketName));

            if (!bucketExists)
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_settings.BucketName));

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(fileName)
                .WithStreamData(content)
                .WithObjectSize(content.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs);

            _logger.LogInformation("Arquivo {FileName} enviado para o bucket {Bucket}", fileName, _settings.BucketName);
            return ResultadoDto<string>.Ok(fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar arquivo {FileName} para o MinIO", fileName);
            return ResultadoDto<string>.Falha("UPLOAD_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<Stream>> DownloadAsync(string blobName)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(blobName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minioClient.GetObjectAsync(getObjectArgs);
            memoryStream.Position = 0;

            _logger.LogInformation("Arquivo {BlobName} baixado do bucket {Bucket}", blobName, _settings.BucketName);
            return ResultadoDto<Stream>.Ok(memoryStream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao baixar arquivo {BlobName} do MinIO", blobName);
            return ResultadoDto<Stream>.Falha("DOWNLOAD_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<bool>> RemoverAsync(string blobName)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(blobName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);

            _logger.LogInformation("Arquivo {BlobName} removido do bucket {Bucket}", blobName, _settings.BucketName);
            return ResultadoDto<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover arquivo {BlobName} do MinIO", blobName);
            return ResultadoDto<bool>.Falha("REMOVE_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<string>> GetUrlAsync(string blobName)
    {
        try
        {
            var presignedUrl = await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(blobName)
                .WithExpiry(3600));

            return ResultadoDto<string>.Ok(presignedUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL para {BlobName}", blobName);
            return ResultadoDto<string>.Falha("URL_ERROR", ex.Message);
        }
    }
}
