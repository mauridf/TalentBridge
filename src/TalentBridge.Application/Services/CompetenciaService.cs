using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class CompetenciaService : ICompetenciaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompetenciaService> _logger;

    public CompetenciaService(IUnitOfWork unitOfWork, ILogger<CompetenciaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<CompetenciaResponseDto>> CriarCompetencia(CriarCompetenciaRequestDto request)
    {
        _logger.LogInformation("Criando competência: {Nome}", request.Nome);

        if (string.IsNullOrWhiteSpace(request.Nome))
            return ResultadoDto<CompetenciaResponseDto>.Falha("NOME_OBRIGATORIO", "O nome da competência é obrigatório.");

        var existe = await _unitOfWork.Competencias.ExistsAsync(c => c.Nome == request.Nome.Trim() && c.EmpresaId == request.EmpresaId);
        if (existe)
            return ResultadoDto<CompetenciaResponseDto>.Falha("COMPETENCIA_EXISTENTE", "Já existe uma competência com este nome.");

        var competencia = new Competencia(request.Nome, request.EmpresaId);
        await _unitOfWork.Competencias.AddAsync(competencia);
        await _unitOfWork.SaveChangesAsync();

        string? empresaNome = null;
        if (request.EmpresaId.HasValue)
        {
            var empresa = await _unitOfWork.Empresas.GetByIdAsync(request.EmpresaId.Value);
            empresaNome = empresa?.Nome;
        }

        _logger.LogInformation("Competência {CompetenciaId} criada: {Nome}", competencia.Id, competencia.Nome);

        return ResultadoDto<CompetenciaResponseDto>.Ok(new CompetenciaResponseDto
        {
            Id = competencia.Id,
            Nome = competencia.Nome,
            EmpresaId = competencia.EmpresaId,
            EmpresaNome = empresaNome
        });
    }

    public async Task<ResultadoDto<List<CompetenciaResponseDto>>> Buscar(string? nome, Guid? empresaId)
    {
        _logger.LogInformation("Buscando competências. Nome: {Nome}, EmpresaId: {EmpresaId}", nome, empresaId);

        IEnumerable<Competencia> competencias;

        if (!string.IsNullOrWhiteSpace(nome) && empresaId.HasValue)
            competencias = await _unitOfWork.Competencias.FindAsync(c =>
                c.Nome.Contains(nome.Trim()) && c.EmpresaId == empresaId.Value);
        else if (!string.IsNullOrWhiteSpace(nome))
            competencias = await _unitOfWork.Competencias.FindAsync(c => c.Nome.Contains(nome.Trim()));
        else if (empresaId.HasValue)
            competencias = await _unitOfWork.Competencias.FindAsync(c => c.EmpresaId == empresaId.Value);
        else
            competencias = await _unitOfWork.Competencias.GetAllAsync();

        var result = competencias.Select(c => new CompetenciaResponseDto
        {
            Id = c.Id,
            Nome = c.Nome,
            EmpresaId = c.EmpresaId,
            EmpresaNome = c.Empresa?.Nome
        }).ToList();

        return ResultadoDto<List<CompetenciaResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<bool>> Remover(Guid id)
    {
        _logger.LogInformation("Removendo competência {CompetenciaId}", id);

        var competencia = await _unitOfWork.Competencias.GetByIdAsync(id);
        if (competencia == null)
            return ResultadoDto<bool>.Falha("COMPETENCIA_NAO_ENCONTRADA", "Competência não encontrada.");

        _unitOfWork.Competencias.Remove(competencia);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Competência {CompetenciaId} removida", id);

        return ResultadoDto<bool>.Ok(true);
    }
}
