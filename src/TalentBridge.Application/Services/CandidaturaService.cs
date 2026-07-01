using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Candidatura;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de candidaturas
/// </summary>
public class CandidaturaService : ICandidaturaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CandidaturaService> _logger;

    public CandidaturaService(IUnitOfWork unitOfWork, ILogger<CandidaturaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova candidatura
    /// </summary>
    public async Task<Result<CandidaturaResponseDto>> CriarAsync(
        Guid candidatoId,
        CriarCandidaturaRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Verificar se a vaga existe e está aberta
        var vaga = await _unitOfWork.Vagas.GetByIdAsync(request.VagaId, cancellationToken);
        if (vaga == null)
            return Result.Fail<CandidaturaResponseDto>("VAGA_NAO_ENCONTRADA");

        if (!vaga.EstaNoPeriodoCandidatura())
            return Result.Fail<CandidaturaResponseDto>("VAGA_FORA_PERIODO_CANDIDATURA");

        // Verificar se já existe candidatura
        var existeCandidatura = await _unitOfWork.Candidaturas
            .ExisteCandidaturaAsync(request.VagaId, candidatoId, cancellationToken);

        if (existeCandidatura)
            return Result.Fail<CandidaturaResponseDto>("CANDIDATURA_JA_EXISTENTE");

        // Verificar se o candidato existe
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<CandidaturaResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        // Criar candidatura
        var candidatura = new Candidatura(
            vagaId: request.VagaId,
            candidatoId: candidatoId,
            origem: request.Origem);

        await _unitOfWork.Candidaturas.AddAsync(candidatura, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Candidatura criada: Vaga={VagaId}, Candidato={CandidatoId}, Protocolo={Protocolo}",
            request.VagaId, candidatoId, candidatura.Protocolo);

        // Buscar dados relacionados para resposta
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(vaga.EmpresaId, cancellationToken);

        var responseDto = new CandidaturaResponseDto
        {
            Id = candidatura.Id,
            VagaId = vaga.Id,
            VagaTitulo = vaga.Titulo,
            EmpresaNome = empresa?.Nome ?? "",
            CandidatoId = candidato.Id,
            CandidatoNome = candidato.Nome,
            CandidatoEmail = candidato.Email,
            Protocolo = candidatura.Protocolo,
            CreatedAt = candidatura.CreatedAt
        };

        return Result.Ok(responseDto);
    }

    /// <summary>
    /// Verifica se já existe candidatura
    /// </summary>
    public async Task<Result<bool>> VerificarCandidaturaExistenteAsync(
        Guid vagaId,
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var existe = await _unitOfWork.Candidaturas
            .ExisteCandidaturaAsync(vagaId, candidatoId, cancellationToken);

        return Result.Ok(existe);
    }

    /// <summary>
    /// Busca candidaturas com filtros
    /// </summary>
    public async Task<Result<IEnumerable<CandidaturaResponseDto>>> BuscarAsync(
        BuscarCandidaturasRequestDto request,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Candidatura> candidaturas;

        if (request.VagaId.HasValue)
        {
            candidaturas = await _unitOfWork.Candidaturas
                .GetByVagaIdWithCandidatoAsync(request.VagaId.Value, cancellationToken);
        }
        else if (request.CandidatoId.HasValue)
        {
            candidaturas = await _unitOfWork.Candidaturas
                .GetByCandidatoIdWithVagaAsync(request.CandidatoId.Value, cancellationToken);
        }
        else
        {
            candidaturas = await _unitOfWork.Candidaturas.GetAllAsync(cancellationToken: cancellationToken);
        }

        // Aplicar filtros adicionais
        if (request.ApenasContratados.HasValue && request.ApenasContratados.Value)
            candidaturas = candidaturas.Where(c => c.Contratado);

        if (request.EntrevistaRealizada.HasValue)
            candidaturas = candidaturas.Where(c => c.EntrevistaRealizada == request.EntrevistaRealizada.Value);

        var dtos = candidaturas.Select(c => MapearParaResponseDto(c));
        return Result.Ok(dtos);
    }

    /// <summary>
    /// Busca candidaturas de uma vaga
    /// </summary>
    public async Task<Result<IEnumerable<CandidaturaResponseDto>>> GetByVagaAsync(
        Guid vagaId,
        CancellationToken cancellationToken = default)
    {
        var candidaturas = await _unitOfWork.Candidaturas
            .GetByVagaIdWithCandidatoAsync(vagaId, cancellationToken);

        var dtos = candidaturas.Select(c => MapearParaResponseDto(c));
        return Result.Ok(dtos);
    }

    /// <summary>
    /// Busca candidaturas de um candidato
    /// </summary>
    public async Task<Result<IEnumerable<CandidaturaResponseDto>>> GetByCandidatoAsync(
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var candidaturas = await _unitOfWork.Candidaturas
            .GetByCandidatoIdWithVagaAsync(candidatoId, cancellationToken);

        var dtos = candidaturas.Select(c => MapearParaResponseDto(c));
        return Result.Ok(dtos);
    }

    /// <summary>
    /// Agenda uma entrevista
    /// </summary>
    public async Task<Result> AgendarEntrevistaAsync(
        AgendarEntrevistaRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas
            .GetByIdAsync(request.CandidaturaId, cancellationToken);

        if (candidatura == null)
            return Result.Fail("CANDIDATURA_NAO_ENCONTRADA");

        candidatura.AgendarEntrevista(
            dataHora: request.DataHora,
            meio: request.Meio,
            link: request.Link,
            duracaoMinutos: request.DuracaoMinutos);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Entrevista agendada: Candidatura={Id}, Data={Data}",
            request.CandidaturaId, request.DataHora);

        // TODO: Enviar email de confirmação de entrevista

        return Result.Ok();
    }

    /// <summary>
    /// Marca entrevista como realizada
    /// </summary>
    public async Task<Result> MarcarEntrevistaRealizadaAsync(
        Guid candidaturaId,
        CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas
            .GetByIdAsync(candidaturaId, cancellationToken);

        if (candidatura == null)
            return Result.Fail("CANDIDATURA_NAO_ENCONTRADA");

        candidatura.MarcarEntrevistaRealizada();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Entrevista marcada como realizada: Candidatura={Id}", candidaturaId);

        return Result.Ok();
    }

    /// <summary>
    /// Marca candidato como contratado
    /// </summary>
    public async Task<Result> ContratarAsync(
        Guid candidaturaId,
        CancellationToken cancellationToken = default)
    {
        var candidatura = await _unitOfWork.Candidaturas
            .GetByIdAsync(candidaturaId, cancellationToken);

        if (candidatura == null)
            return Result.Fail("CANDIDATURA_NAO_ENCONTRADA");

        candidatura.Contratar();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Candidato contratado: Candidatura={Id}", candidaturaId);

        // TODO: Notificar candidato sobre contratação

        return Result.Ok();
    }

    /// <summary>
    /// Mapeia entidade para DTO de resposta
    /// </summary>
    private static CandidaturaResponseDto MapearParaResponseDto(Candidatura candidatura)
    {
        return new CandidaturaResponseDto
        {
            Id = candidatura.Id,
            VagaId = candidatura.VagaId,
            VagaTitulo = candidatura.Vaga?.Titulo ?? "",
            EmpresaNome = candidatura.Vaga?.Empresa?.Nome ?? "",
            CandidatoId = candidatura.CandidatoId,
            CandidatoNome = candidatura.Candidato?.Nome ?? "",
            CandidatoEmail = candidatura.Candidato?.Email ?? "",
            Protocolo = candidatura.Protocolo,
            Contratado = candidatura.Contratado,
            EntrevistaRealizada = candidatura.EntrevistaRealizada,
            DataHoraEntrevista = candidatura.DataHoraEntrevista,
            MeioEntrevista = candidatura.MeioEntrevista,
            LinkEntrevista = candidatura.LinkEntrevista,
            DuracaoEntrevistaMinutos = candidatura.DuracaoEntrevistaMinutos,
            CreatedAt = candidatura.CreatedAt
        };
    }
}
