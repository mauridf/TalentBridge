using FluentResults;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.LandingPage;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de landing pages
/// </summary>
public class LandingPageService : ILandingPageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LandingPageService> _logger;

    public LandingPageService(IUnitOfWork unitOfWork, ILogger<LandingPageService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Obtém landing page da empresa com vagas abertas
    /// </summary>
    public async Task<Result<LandingPageResponseDto>> ObterLandingPageAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken);
        if (empresa == null)
            return Result.Fail<LandingPageResponseDto>("EMPRESA_NAO_ENCONTRADA");

        var vagas = await _unitOfWork.Vagas.GetByEmpresaIdAsync(empresaId, cancellationToken);
        var vagasAbertas = vagas
            .Where(v => v.Status == StatusVagaEnum.Aberta && !v.Encerrada && v.EstaNoPeriodoCandidatura())
            .OrderByDescending(v => v.CreatedAt)
            .ToList();

        var response = new LandingPageResponseDto
        {
            EmpresaId = empresa.Id,
            EmpresaNome = empresa.Nome,
            EmpresaWebsite = empresa.Website,
            EmpresaDescricao = empresa.Comentarios,
            EmpresaMissao = empresa.Missao,
            EmpresaVisao = empresa.Visao,
            EmpresaValores = empresa.Valores,
            TotalVagasAbertas = vagasAbertas.Count,
            Vagas = vagasAbertas.Select(v => new VagaLandingPageDto
            {
                VagaId = v.Id,
                Titulo = v.Titulo,
                Cargo = v.Cargo,
                DescricaoResumida = v.Descricao?.Length > 200 ? v.Descricao[..200] + "..." : v.Descricao,
                Cidade = v.Cidade,
                Estado = v.Estado,
                Salario = v.Salario,
                TipoVaga = v.TipoVaga.ToString(),
                DataPublicacao = v.CreatedAt,
                DataLimite = v.DataCandidaturaFim
            }).ToList()
        };

        return Result.Ok(response);
    }

    /// <summary>
    /// Obtém detalhe de uma vaga
    /// </summary>
    public async Task<Result<VagaDetalheLandingPageDto>> ObterDetalheVagaAsync(
        Guid vagaId,
        CancellationToken cancellationToken = default)
    {
        var vaga = await _unitOfWork.Vagas.GetByIdAsync(vagaId, cancellationToken);
        if (vaga == null)
            return Result.Fail<VagaDetalheLandingPageDto>("VAGA_NAO_ENCONTRADA");

        var empresa = await _unitOfWork.Empresas.GetByIdAsync(vaga.EmpresaId, cancellationToken);

        var response = new VagaDetalheLandingPageDto
        {
            VagaId = vaga.Id,
            EmpresaId = vaga.EmpresaId,
            EmpresaNome = empresa?.Nome ?? "",
            Titulo = vaga.Titulo,
            Cargo = vaga.Cargo,
            Descricao = vaga.Descricao,
            Atividades = vaga.Atividades,
            Beneficios = vaga.Beneficios,
            DiferenciaisConsiderados = vaga.DiferenciaisConsiderados,
            Salario = vaga.Salario,
            Cidade = vaga.Cidade,
            Estado = vaga.Estado,
            TipoVaga = vaga.TipoVaga.ToString(),
            Status = vaga.Status.ToString(),
            QuantidadeVagas = vaga.QuantidadeVagas,
            DataPublicacao = vaga.CreatedAt,
            DataLimite = vaga.DataCandidaturaFim,
            AceitaCandidatura = vaga.EstaNoPeriodoCandidatura()
        };

        return Result.Ok(response);
    }

    /// <summary>
    /// Busca empresa por slug (nome formatado)
    /// </summary>
    public async Task<Result<LandingPageResponseDto>> ObterPorSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        // Buscar empresa cujo nome formatado corresponde ao slug
        var empresas = await _unitOfWork.Empresas.GetAllAsync(cancellationToken: cancellationToken);
        var empresa = empresas.FirstOrDefault(e =>
            e.Nome.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("ç", "c")
                .Replace("ã", "a")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u") == slug.ToLowerInvariant());

        if (empresa == null)
            return Result.Fail<LandingPageResponseDto>("EMPRESA_NAO_ENCONTRADA");

        return await ObterLandingPageAsync(empresa.Id, cancellationToken);
    }
}
