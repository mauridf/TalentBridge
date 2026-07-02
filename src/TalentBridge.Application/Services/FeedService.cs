using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class FeedService : IFeedService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FeedService> _logger;

    public FeedService(IUnitOfWork unitOfWork, ILogger<FeedService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<string>> GerarFeedXml()
    {
        _logger.LogInformation("Gerando feed XML de vagas.");

        var vagas = await _unitOfWork.Vagas.FindAsync(v =>
            v.Status == Domain.Enums.StatusVagaEnum.Aberta && !v.Encerrada &&
            v.DataCandidaturaInicio <= DateTime.UtcNow &&
            v.DataCandidaturaFim >= DateTime.UtcNow);

        var xml = new System.Text.StringBuilder();
        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine("<feed xmlns=\"http://www.w3.org/2005/Atom\" xmlns:jobs=\"http://schema.google.com/jobs/1.0\">");
        xml.AppendLine("<title>TalentBridge - Vagas de Emprego</title>");
        xml.AppendLine($"<updated>{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</updated>");
        xml.AppendLine("<id>urn:talentbridge:vagas</id>");

        foreach (var vaga in vagas)
        {
            xml.AppendLine("<entry>");
            xml.AppendLine($"<id>urn:talentbridge:vaga:{vaga.Id}</id>");
            xml.AppendLine($"<title>{System.Security.SecurityElement.Escape(vaga.Titulo)}</title>");
            xml.AppendLine($"<updated>{vaga.UpdatedAt:yyyy-MM-ddTHH:mm:ssZ}</updated>");
            xml.AppendLine($"<jobs:description>{System.Security.SecurityElement.Escape(vaga.Descricao)}</jobs:description>");
            xml.AppendLine($"<jobs:location>{System.Security.SecurityElement.Escape(vaga.Cidade)}, {System.Security.SecurityElement.Escape(vaga.Estado)}</jobs:location>");
            xml.AppendLine($"<jobs:company>{System.Security.SecurityElement.Escape(vaga.Empresa?.Nome ?? string.Empty)}</jobs:company>");
            xml.AppendLine("</entry>");
        }

        xml.AppendLine("</feed>");

        _logger.LogInformation("Feed XML gerado com {Quantidade} vagas.", vagas.Count());

        return ResultadoDto<string>.Ok(xml.ToString());
    }
}
